using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour{
    public string Id;
    public Sprite sprite;
    public string itemName;

    void Start()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null) sprite = rend.sprite; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                player.PickUp(this);
                Destroy(gameObject);
            }
        }
    }
}
