using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Cauldron : MonoBehaviour {
    public static Cauldron instance;
    List<ItemTile> brewItems;
    bool isHovering;

    void Start()
    {
        instance = this;
        brewItems = new List<ItemTile>();
    }

    public void AddItemToBrew(ItemTile item)
    {
        brewItems.Add(item);
    }

    public void RemoveFromBrew()
    {

    }

    public void ClearBrew()
    {

    }
}
