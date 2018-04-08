using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public string Id;
    public string itemName;
    public string description;
    public int quantity;
    public int value = 0;
    public Sprite sprite;
    public List<Property> properties = new List<Property>();
    public Potion potion = null;
    public CrossbowUpgrade.Upgrade upgrade;
    public bool consumable;

    public bool Use() {
        if (consumable)
        {
            int power = 0;
            
            //Healing consumables
            if ((power = CheckForProperty(Property.Type.Healing)) > 0)
            {
                Player.instance.AddHealth(power);
                return true;
            }

            //Status Effects
            ApplyStatus(Player.instance.gameObject);
        }
        return false;
    }

    public void ApplyStatus(GameObject target)
    {
        for (int i = 0; i < properties.Count; i++)
        {
            switch (properties[i].type)
            {
                case Property.Type.Fire:
                    target.AddComponent<Burning>();
                    break;

                case Property.Type.Ice:
                    target.AddComponent<Frozen>();
                    break;
            }
        }
    }

    private int CheckForProperty(Property.Type type)
    {
        for (int i = 0; i < properties.Count; i++)
        {
            if (properties[i].type == type)
            {
                return properties[i].power;
            }
        }

        return -1;
    }
}


