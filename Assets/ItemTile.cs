using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTile : MonoBehaviour {
    public Item item;
    public Image rend;

    public void SetItem(Item _item)
    {
        item = _item;
        rend.enabled = true;
        rend.sprite = item.sprite;
        rend.preserveAspect = true;
    }

    public void RemoveItem()
    {
        item = null;
        rend.enabled = false;
    }

    public void ShowDescription()
    {
        Debug.Log("Showing Description!");
        if(item != null)
        {
            HoverBox.instance.ShowDescription(item);
        }
    }

    public void HideDescription()
    {
        if(item != null)
        {
            HoverBox.instance.HideDescription();
        }
    }
}
