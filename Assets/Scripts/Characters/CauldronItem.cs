using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronItem : MonoBehaviour {
    public Item item;

    public void SetItem(Item _item)
    {
        item = _item;
        GetComponent<Image>().sprite = item.sprite;
    }

	public void StartDrag() {
		Inventory.instance.StartItemDrag (item);
        Cauldron.instance.RemoveFromBrew(this);
		Destroy (gameObject);
	}
}
