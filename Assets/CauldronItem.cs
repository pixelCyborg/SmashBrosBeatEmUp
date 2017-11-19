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
}
