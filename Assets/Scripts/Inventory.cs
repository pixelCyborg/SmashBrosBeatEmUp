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
        itemCursor.enabled = false;
        myCanvas = GetComponentInParent<Canvas>();
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

        if(draggedItem != null)
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

    public void StartItemDrag(ItemTile itemTile)
    {
        origTile = itemTile;
		StartItemDrag (itemTile.item);
    }

	public void StartItemDrag(Item _item) {
		HoverBox.instance.gameObject.SetActive(false);
		itemCursor.enabled = true;
		draggedItem = _item;
		itemCursor.sprite = draggedItem.sprite;
	}

    void EndItemDrag()
    {
        if(hoveredTile != null)
        {
            if(hoveredTile.item != null)
            {
                origTile.SetItem(hoveredTile.item);
                hoveredTile.SetItem(draggedItem);
                HoverBox.instance.ShowDescription(draggedItem);
            }
            hoveredTile.SetItem(draggedItem);
        }
        else if(Cauldron.instance.isHovering)
        {
			HoverBox.instance.HideDescription ();
            Cauldron.instance.AddItemToBrew(draggedItem);
        }
        else if(origTile != null)
        {
            origTile.SetItem(draggedItem);
        }

        itemCursor.enabled = false;
        draggedItem = null;
        origTile = null;
        HoverBox.instance.gameObject.SetActive(true);
        HoverBox.instance.UpdateBoxPos();
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

    public static Vector2 MouseToUiPos()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        return myCanvas.transform.TransformPoint(pos);
    }
}
