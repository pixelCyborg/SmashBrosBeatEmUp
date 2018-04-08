using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTile : MonoBehaviour {
    public Item item;

    public Image rend;
    private Image background;
    private Color origColor;

    public delegate void OnSetItem(Item setItem);
    public delegate void OnRemoveItem(Item removeItem);
    public OnSetItem onSetItem;
    public OnRemoveItem onRemoveItem;

    void Start()
    {
        item = null;
        background = GetComponent<Image>();
        origColor = background.color;
    }

    public void SetItem(Item _item)
    {
        item = _item;
        rend.enabled = true;
        rend.sprite = item.sprite;
        rend.preserveAspect = true;

        if(onSetItem != null) onSetItem(item);
    }

    public void RemoveItem()
    {
        if(onRemoveItem != null) onRemoveItem(item);
        item = null;
        rend.enabled = false;
    }

    public void StartDrag()
    {
        //If right click
        if (Input.GetMouseButton(1))
        {
            if(item.Use())
            {
                RemoveItem();
                HoverBox.instance.HideDescription();
            }
        }
        else
        {
            Inventory.instance.StartItemDrag(this);
            RemoveItem();
        }
    }

    public void OnHoverStart()
    {
        Highlight();
        Inventory.hoveredTile = this;

        if (item != null)
        {
            HoverBox.instance.ShowDescription(item);
        }
    }

    public void OnHoverEnd()
    {
        Inventory.hoveredTile = null;
        Unhighlight();
        if(item != null)
        {
            HoverBox.instance.HideDescription();
        }
    }

    public void Highlight()
    {
        background.color = Color.white;
    }

    public void Unhighlight()
    {
        background.color = origColor;
    }
}
