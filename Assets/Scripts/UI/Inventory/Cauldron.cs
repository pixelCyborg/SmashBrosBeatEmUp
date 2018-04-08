using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Cauldron : MonoBehaviour {
    Item item;
    public Sprite[] potionSpriteBank;
    public GameObject cauldronItemPrefab;
    public static Cauldron instance;
    List<CauldronItem> brewItems;
    public bool isHovering;

    void Start()
    {
        instance = this;
        brewItems = new List<CauldronItem>();
    }

    public void StartHover()
    {
        isHovering = true;
    }

    public void EndHover()
	{
        isHovering = false;
    }

    public void AddItemToBrew(Item item)
    {
        GameObject go = Instantiate(cauldronItemPrefab, Inventory.MouseToUiPos(), Quaternion.identity, transform);
        CauldronItem newItem = go.GetComponent<CauldronItem>();
        newItem.SetItem(item);
        brewItems.Add(newItem);
    }

    public void RemoveFromBrew(CauldronItem item)
    {
        brewItems.Remove(item);
    }

    public void ClearBrew()
    {
        for(int i = 0; i < brewItems.Count; i++)
        {
            Destroy(brewItems[i].gameObject);
        }

        brewItems = new List<CauldronItem>();
    }

    public void Brew()
    {
        if (brewItems.Count == 0) return;

        item = new Item();

        List<Property> potProperties = new List<Property>();
        for(int i = 0; i < brewItems.Count; i++)
        {
            foreach(Property prop in brewItems[i].item.properties)
            {
                int sharedProperty = potProperties.FindIndex(x => x.type == prop.type);
                if(sharedProperty != -1)
                {
                    potProperties[sharedProperty].power += prop.power;
                }
                else
                {
                    potProperties.Add(prop);
                }
            }
            //potProperties.AddRange(brewItems[i].item.properties);
        }

        item.description = "";

        if (potProperties != null && potProperties.Count > 0)
        {
            item.potion = new Potion();
            item.potion.properties = potProperties.ToArray();
            item.properties = potProperties;
            for (int i = 0; i < item.potion.properties.Length; i++)
            {
                item.description += "-" + item.potion.properties[i].type + " " + item.potion.properties[i].power + "\n";
            }
        }

        item.sprite = potionSpriteBank[Random.Range(0, potionSpriteBank.Length)];
        item.itemName = "Potion";
        item.consumable = true;

        Inventory.instance.AddToInventory(item);
        ClearBrew();
    }
}
