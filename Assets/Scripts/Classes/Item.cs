using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public string Id;
    public string itemName;
    public string description;
    public int value = 0;
    public Sprite sprite;
    public List<Property> properties = new List<Property>();
    public Potion potion = null;
    public CrossbowUpgrade.Upgrade upgrade;
}
