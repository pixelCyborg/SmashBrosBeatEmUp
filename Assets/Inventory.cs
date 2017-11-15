using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    RectTransform inventoryPanel;
    List<Item> inventory;
	
    void Start()
    {
        inventoryPanel = transform.GetChild(0).GetComponent<RectTransform>();
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
        }
	}
}
