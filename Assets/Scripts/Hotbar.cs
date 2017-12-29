using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour {
    int currentIndex;
    ItemTile[] hotbarTiles;

    public Transform inventoryHotbar;
    ItemTile[] inventoryHotbarTiles;

	// Use this for initialization
	void Start () {
        hotbarTiles = new ItemTile[transform.childCount];
        inventoryHotbarTiles = new ItemTile[inventoryHotbar.childCount];

        for (int i = 0; i < hotbarTiles.Length; i++)
        {
            hotbarTiles[i] = transform.GetChild(i).GetComponent<ItemTile>();
        }

        for (int i = 0; i < inventoryHotbarTiles.Length; i++)
        {
            inventoryHotbarTiles[i] = inventoryHotbar.GetChild(i).GetComponent<ItemTile>();
        }

        SetIndex(0);
    }

    public void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            if (currentIndex == hotbarTiles.Length - 1)
            {
                SetIndex(0);
            }
            else {
                SetIndex(currentIndex + 1);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            if (currentIndex == 0)
            {
                SetIndex(hotbarTiles.Length - 1);
            }
            else {
                SetIndex(currentIndex - 1);
            }
        }
    }

    public void UpdateHotbar()
    {
        for(int i = 0; i < inventoryHotbarTiles.Length; i++)
        {
            if(inventoryHotbarTiles[i].item != null)
            {
                hotbarTiles[i].SetItem(inventoryHotbarTiles[i].item);
            }
            else
            {
                hotbarTiles[i].RemoveItem();
            }
        }
    }

    public void SetIndex(int newIndex)
    {
        hotbarTiles[currentIndex].Unhighlight();
        currentIndex = newIndex;
        hotbarTiles[currentIndex].Highlight();

    }

    public ItemTile CurrentTile()
    {
        return hotbarTiles[currentIndex];
    }

    /*
    public void WriteInventoryHotbar()
    {
        for (int i = 0; i < hotbarTiles.Length; i++)
        {
            if (hotbarTiles[i].item != null)
            {
                inventoryHotbarTiles[i].SetItem(hotbarTiles[i].item);
            }
            else
            {
                inventoryHotbarTiles[i].RemoveItem();
            }
        }
    }
    */
}
