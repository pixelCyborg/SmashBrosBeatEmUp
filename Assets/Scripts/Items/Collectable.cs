using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable {
    private static Player player;
    public Item item = new Item();

    void Start()
    {
        if (gameObject.name.Contains("(Clone)")) gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);

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

        if(item.quantity < 1)
        {
            item.quantity = 1;
        }

        description = item.itemName;

        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.enabled)
        {
            if (player != null && collision.tag == "Player")
            {
                player.PickUp(this);
            }
        }
    }
}
