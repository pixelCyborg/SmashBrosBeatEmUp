using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Cauldron : MonoBehaviour {
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

    public void RemoveFromBrew()
    {

    }

    public void ClearBrew()
    {

    }
}
