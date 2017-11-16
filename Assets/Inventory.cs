using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    RectTransform inventoryPanel;
    CanvasGroup group;

    public Transform hotbarParent;
    public Transform inventoryParent;
    ItemTile[] hotbarTiles;
    ItemTile[] inventoryTiles;
    List<Item> heldItems;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        heldItems = new List<Item>();
        hotbarTiles = new ItemTile[hotbarParent.childCount];
        inventoryTiles = new ItemTile[inventoryParent.childCount];

        for(int i = 0; i < hotbarTiles.Length; i++)
        {
            hotbarTiles[i] = hotbarParent.GetChild(i).GetComponent<ItemTile>();
        }

        for (int i = 0; i < inventoryTiles.Length; i++)
        {
            inventoryTiles[i] = inventoryParent.GetChild(i).GetComponent<ItemTile>();
        }

        group = GetComponent<CanvasGroup>();
        inventoryPanel = transform.GetChild(0).GetComponent<RectTransform>();
        ToggleOff();
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
        {
            if (group.alpha == 0) {
                ToggleOn();
            }
            else
            {
                ToggleOff();
            }
        }
	}

    public void AddToInventory(Item item)
    {
        heldItems.Add(item);
        ItemTile tile = FirstAvailableTile(item);
        tile.SetItem(item);
    }

    ItemTile FirstAvailableTile(Item item)
    {
        for(int i = 0; i < hotbarTiles.Length; i++)
        {
            if (hotbarTiles[i].item == null) return hotbarTiles[i];
        }

        for(int i = 0; i < inventoryTiles.Length; i++)
        {
            if (inventoryTiles[i].item == null) return inventoryTiles[i];
        }

        return null;
    }

    void ToggleOn() {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    void ToggleOff()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}
