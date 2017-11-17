using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTile : MonoBehaviour {
    public Item item;
    public Image rend;
    private Image background;
    private Color origColor;

    void Start()
    {
        background = GetComponent<Image>();
        origColor = background.color;
    }

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

    public void StartDrag()
    {
        Inventory.instance.StartItemDrag(this);
        RemoveItem();
    }

    public void OnHoverStart()
    {
        background.color = Color.white;
        Inventory.hoveredTile = this;
       
        if(item != null)
        {
            HoverBox.instance.ShowDescription(item);
        }
    }

    public void OnHoverEnd()
    {
        background.color = origColor;
        Inventory.hoveredTile = null;
        if(item != null)
        {
            HoverBox.instance.HideDescription();
        }
    }
}
