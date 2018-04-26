using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    static Canvas myCanvas;
    public static Inventory instance;
    RectTransform inventoryPanel;
    CanvasGroup group;

    public static ItemTile hoveredTile;
    private Item draggedItem;
    private ItemTile origTile;
    public Image itemCursor;

    public Transform inventoryParent;

    ItemTile[] inventoryTiles;
    List<Item> heldItems;

    public static int coin = 0;
    public Text coinMeter;
    private static Text CoinMeter;
    public static bool open = false;

    void Awake()
    {
        instance = this;
        CoinMeter = coinMeter;
    }

    void Start()
    {
        myCanvas = GetComponentInParent<Canvas>();
        heldItems = new List<Item>();
        inventoryTiles = new ItemTile[inventoryParent.childCount];

        for (int i = 0; i < inventoryTiles.Length; i++)
        {
            inventoryTiles[i] = inventoryParent.GetChild(i).GetComponent<ItemTile>();
        }

        PopulateInventory(heldItems.ToArray());

        group = GetComponent<CanvasGroup>();
        inventoryPanel = transform.GetChild(0).GetComponent<RectTransform>();
        ToggleOff();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Toggle();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (group.alpha >= 0.9f)
            {
                {
                    ToggleOff();
                }
            }
        }

        if (draggedItem != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            itemCursor.transform.position = myCanvas.transform.TransformPoint(pos);

            if (Input.GetMouseButtonUp(0))
            {
                EndItemDrag();
            }
        }
    }

    public void Toggle(bool expanded = false)
    {
        if (group.alpha >= 0.9f)
        {
            ToggleOff();
        }
        else
        {
            ToggleOn(expanded);
        }
    }

    public static void AddCoin(int amount = 1)
    {
        coin += amount;
        CoinMeter.text = "Coin: " + coin;
    }

    public void AddToInventory(Item item)
    {
        for (int i = 0; i < heldItems.Count; i++)
        {
            //Update a tile
            if (heldItems[i].itemName == item.itemName)
            {
                heldItems[i].quantity++;
                inventoryTiles[i].SetItem(heldItems[i]);
                return;
            }
        }

        //Add to inventory
        heldItems.Add(item);
        ItemTile tile = FirstAvailableTile(item);
        tile.SetItem(item);
    }

    public ItemTile FirstAvailableTile(Item item)
    {
        for (int i = 0; i < inventoryTiles.Length; i++)
        {
            if (inventoryTiles[i].item == null) return inventoryTiles[i];
        }

        return null;
    }

    public void StartItemDrag(ItemTile itemTile)
    {
        origTile = itemTile;
        StartItemDrag(itemTile.item);
    }

    public void StartItemDrag(Item _item) {
        if (_item == null) return;
        HoverBox.instance.gameObject.SetActive(false);
        itemCursor.enabled = true;
        draggedItem = _item;
        itemCursor.sprite = draggedItem.sprite;
    }

    void EndItemDrag()
    {
        if (hoveredTile != null)
        {
            if (hoveredTile.item != null)
            {
                origTile.SetItem(hoveredTile.item);

                hoveredTile.onRemoveItem(hoveredTile.item);
                hoveredTile.SetItem(draggedItem);

                HoverBox.instance.ShowDescription(draggedItem);
            }
            else hoveredTile.SetItem(draggedItem);
        }
        else if (Cauldron.instance.isHovering)
        {
            HoverBox.instance.HideDescription();
            Cauldron.instance.AddItemToBrew(draggedItem);
        }
        else if (origTile != null)
        {
            origTile.SetItem(draggedItem);
        }

        itemCursor.enabled = false;
        draggedItem = null;
        origTile = null;
        //HoverBox.instance.gameObject.SetActive(true);
        //HoverBox.instance.UpdateBoxPos();
    }

    public void ToggleOn(bool expanded) {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
        open = true;
        //Cauldron.instance.gameObject.SetActive(expanded);
    }

    public void ToggleOff()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
        open = false;
    }

    public Item[] GetInventoryItems() {
		return heldItems.ToArray();
	}

	public void PopulateInventory(Item[] inventoryItems) {
		for (int i = 0; i < inventoryTiles.Length; i++) {
            if (i < inventoryItems.Length)
            {
                inventoryTiles[i].SetItem(inventoryItems[i]);
            }
            else
            {
                inventoryTiles[i].RemoveItem();
            }
		}
	}

    public static Vector2 MouseToUiPos()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        return myCanvas.transform.TransformPoint(pos);
    }
}
