using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop : System.IComparable<ItemDrop>
{
    public GameObject item;
    public int dropChance;

    public int CompareTo(ItemDrop other)
    {
        return dropChance.CompareTo(other.dropChance);
    }
}
