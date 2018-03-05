using System;
using System.Collections;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsLadder;                  // A mask determining what is ladder to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    RigidbodyConstraints2D origConstraints;

    //Custom Movement Stuff
    public bool isClimbing = false;
    private bool checkingGround = true;
    private bool dashing = false;
    private bool canDash = false;
    private Collider2D col;

    public float dashVelocity;
    private float origGravity;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        origConstraints = m_Rigidbody2D.constraints;

        origGravity = m_Rigidbody2D.gravityScale;
    }


    private void FixedUpdate()
    {
        if (checkingGround)
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
                    m_Grounded = true;
            }
            //m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            //m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }
    }

    public void StartClimbing()
    {
        isClimbing = true;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void StopClimbing()
    {
        isClimbing = false;
        m_Rigidbody2D.constraints = origConstraints;
    }


    public void Move(float move, float climb, bool crouch, bool jump, bool dash)
    {
        if (dash && canDash)
        {
            canDash = false;
            Dash();
        }
        if (dashing) return;

        if (m_Grounded) canDash = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsLadder);
        if (isClimbing && m_Grounded)
        {
            bool onLadder = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    onLadder = true;
            }

            if (!onLadder) StopClimbing();
        }
        if (isClimbing && Mathf.Abs(climb) > 0.1f)
        {
            transform.position += Vector3.up * climb * Time.deltaTime * m_MaxSpeed;
        }
        //only control the player if grounded or airControl is turned on
        else if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * m_CrouchSpeed : move);
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if ((m_Grounded || isClimbing) && jump)
        {
            if (isClimbing)
            {
                StopClimbing();
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            }

            // Add a vertical force to the player.
            m_Grounded = false;
            //Drop through one-way-platforms
            if (climb < 0)
            {
                StartCoroutine(DropThroughPlatform());
            }
            else
            {
                Vector2 tempVel = m_Rigidbody2D.velocity;
                tempVel.y = 0;
                m_Rigidbody2D.velocity = tempVel;
                //m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }

    public void Dash()
    {
        StartCoroutine(_Dash());
    }

    IEnumerator _Dash()
    {
        dashing = true;
        m_Rigidbody2D.gravityScale = 0.0f;
        m_Rigidbody2D.velocity = Vector3.right * dashVelocity * transform.localScale.x;
        yield return new WaitForSeconds(0.2f);
        dashing = false;
        m_Rigidbody2D.gravityScale = origGravity ;
    }

    IEnumerator DropThroughPlatform()
    {
        checkingGround = false;
        Physics2D.IgnoreLayerCollision(9, 15);
        col.enabled = false;
        col.enabled = true;
        yield return new WaitForSeconds(0.25f);
        checkingGround = true;
        Physics2D.IgnoreLayerCollision(9, 15, false);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
