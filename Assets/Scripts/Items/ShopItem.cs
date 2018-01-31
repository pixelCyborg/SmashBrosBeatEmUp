using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Interactable {
    public GameObject itemPrefab;
    private Item item;
    public Transform anchor;
    private GameObject collectable;

    private void Start()
    {
        if(item == null)
        {
            if (itemPrefab != null)
            {
                //Spawn a collectable but set it to be only for show
                collectable = (GameObject)Instantiate(itemPrefab, anchor.position, itemPrefab.transform.rotation, anchor);
                collectable.name = itemPrefab.name;
                collectable.GetComponent<Rigidbody2D>().simulated = false;
                StartCoroutine(GetItem());
            }
        }
    }

    internal override void OnSelect()
    {
        base.OnSelect();
        
    }

    internal override void OnDeselect()
    {
        base.OnDeselect();
    }

    IEnumerator GetItem()
    {
        yield return null;
        item = collectable.GetComponent<Collectable>().item;
        description = item.itemName + "\n" + item.value + "g\n\n\n\n" ;
    }

    internal override void OnInteract()
    {
        base.OnInteract();
        if(Inventory.coin >= item.value)
        {
            Inventory.AddCoin(-item.value);
            Inventory.instance.AddToInventory(item);
            InteractionSelector.Deselect();
            Destroy(collectable);
            interactable = false;
        }
    }
}
