using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    Player player;
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;


    private void Awake()
    {
        player = GetComponent<Player>();
        m_Character = GetComponent<PlatformerCharacter2D>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = Input.GetButtonDown("Jump");
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (player.takingDamage) return;
        // Pass all parameters to the character control script.
        m_Character.Move(h, v, crouch, m_Jump);
        m_Jump = false;
    }
}