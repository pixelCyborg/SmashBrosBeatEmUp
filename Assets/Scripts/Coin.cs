using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public AudioClip coinSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if(player != null)
            {
                if (coinSound != null) SoundEffects.PlaySFX(coinSound, 0.5f);
                player.PickUpCoin(this);
                Destroy(gameObject);
            }
        }
    }
}
