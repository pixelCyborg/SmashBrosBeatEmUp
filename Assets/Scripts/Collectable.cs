using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
    public Item item = new Item();

    void Start()
    {
        if (item.properties == null)
        {
            item.properties = new List<Property>();
        }
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null) item.sprite = rend.sprite;

        if (string.IsNullOrEmpty(item.itemName))
        {
            item.itemName = gameObject.name;
        }

        if (string.IsNullOrEmpty(item.description))
        {
            item.description = "";
            for (int i = 0; i < item.properties.Count; i++)
            {
                item.description += "-" + item.properties[i].type + " " + item.properties[i].power + "\n";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                player.PickUp(this);
            }
        }
    }
}
