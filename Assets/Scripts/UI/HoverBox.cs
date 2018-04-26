using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverBox : MonoBehaviour {
    public static HoverBox instance;

    private Sprite origSprite;
    public Image itemIcon;
    public Text itemName;
    public Text itemDescription;
    public Text quantity;

    void Start()
    {
        instance = this;
        origSprite = itemIcon.sprite;
        HideDescription();
    }

    public void ShowDescription(Item item)
    {
        itemIcon.sprite = item.sprite;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        quantity.text = item.quantity.ToString();
        itemIcon.enabled = true;
    }

    public void HideDescription()
    {
        itemIcon.sprite = origSprite;
        itemName.text = "";
        itemDescription.text = "";
        quantity.text = "";
        itemIcon.enabled = false;
    }
}
