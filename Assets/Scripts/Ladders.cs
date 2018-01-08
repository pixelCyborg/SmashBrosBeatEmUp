using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Ladders : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlatformerCharacter2D player = collision.GetComponent<PlatformerCharacter2D>();
            if(player != null)
            {
                player.isClimbing = true;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlatformerCharacter2D player = collision.GetComponent<PlatformerCharacter2D>();
            if (player != null)
            {
                player.isClimbing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlatformerCharacter2D player = collision.GetComponent<PlatformerCharacter2D>();
            if (player != null)
            {
                player.isClimbing = false;
            }
        }
    }
}
