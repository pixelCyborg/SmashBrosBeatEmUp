using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class SimpleBody2D : MonoBehaviour {
    public LayerMask groundMask;
    public Vector2 velocity;
    public RigidbodyConstraints2D constraints;
    private CapsuleCollider2D capsuleCol;
    public float gravityScale = 1.0f;
    private bool _grounded;

    private void Start()
    {
        capsuleCol = GetComponent<CapsuleCollider2D>();
        _grounded = Grounded();
    }

    private void Update()
    {
        _grounded = Grounded();
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + (Vector3)velocity * Time.fixedDeltaTime;

        if (_grounded)
        {
            _grounded = true;
            if (velocity.y <= 0) velocity.y = 0;
        }
        else
        {
            _grounded = false;
            velocity -= -Physics2D.gravity * gravityScale;
        }

        velocity *= 0.98f;
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode2D)
    {
        velocity += force;
    }

    private bool Grounded()
    {
        bool groundHit = false;
        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(transform.position, capsuleCol.size, CapsuleDirection2D.Vertical, 0, -Vector2.down, 0.2f, groundMask);
        for(int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.gameObject != gameObject)
            {
                groundHit = true;
            }
        }

        return groundHit;
    }
}
