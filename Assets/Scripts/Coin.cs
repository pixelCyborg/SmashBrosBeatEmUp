using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public AudioClip coinSound;
    const int COIN_TIMEOUT = 3;

    private void Start()
    {
        StartCoroutine(CoinTimeout());
    }

    IEnumerator CoinTimeout()
    {
        yield return new WaitForSeconds(COIN_TIMEOUT);
        Destroy(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if(player != null)
            {
                player.PickUp(this);
                Destroy(gameObject);
            }
        }
    }
}
