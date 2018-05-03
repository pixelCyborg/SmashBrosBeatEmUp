using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTile : MonoBehaviour {
    public Item item;

    public Image rend;
    public Text itemName;
    public Text quantity;

    private Image background;

    public delegate void OnSetItem(Item setItem);
    public delegate void OnRemoveItem(Item removeItem);
    public OnSetItem onSetItem;
    public OnRemoveItem onRemoveItem;

    private Color origColor;

    void Start()
    {
        background = GetComponent<Image>();
        origColor = background.color;
        item = null;
    }

    public void SetItem(Item _item)
    {
        item = _item;
        rend.enabled = true;
        rend.sprite = item.sprite;
        rend.preserveAspect = true;
        itemName.text = item.itemName;
        quantity.text = item.quantity.ToString();

        if(onSetItem != null) onSetItem(item);
    }

    public void RemoveItem()
    {
        if(onRemoveItem != null) onRemoveItem(item);
        item = null;
        rend.sprite = null;
        rend.enabled = false;
        itemName.text = "-";
        quantity.text = "";
    }


    public void OnClick()
    {
        if (item.Use())
        {
            CanvasManager.instance.audioHandler.PlayInventoryUse();
            if (item.quantity == 1)
            {
                RemoveItem();
            }
            else
            {
                item.quantity -= 1;
                Inventory.instance.PopulateInventory(Inventory.instance.GetInventoryItems());
            }
            HoverBox.instance.HideDescription();
        }
    }

    public void OnHoverStart()
    {
        Highlight();
        Inventory.hoveredTile = this;

        if (item != null)
        {
            CanvasManager.instance.audioHandler.PlayInventoryHover();
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
        background.color = new Color(0.05f, 0.05f, 0.05f);
    }

    public void Unhighlight()
    {
        background.color = origColor;
    }

}
