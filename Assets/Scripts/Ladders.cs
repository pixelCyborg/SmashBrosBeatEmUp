using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Ladders : MonoBehaviour {
    bool isColliding;
    PlatformerCharacter2D player;

    private void Start()
    {
        player = FindObjectOfType<PlatformerCharacter2D>();
    }

    private void Update()
    {
        if (isColliding)
        {
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                player.StartClimbing();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isColliding = false;
            if (player.isClimbing) player.StopClimbing();
        }
    }
}
