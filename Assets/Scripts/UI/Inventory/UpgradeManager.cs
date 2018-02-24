using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    Crossbow crossbow;

    public ItemTile infusionDisplay;
    public ItemTile leftInfusion;
    public ItemTile rightInfusion;
    private bool infusionSwitched = false;

    private List<ItemTile> upgradeTiles = new List<ItemTile>();
    private List<CrossbowUpgrade> currentUpgrades = new List<CrossbowUpgrade>();

	// Use this for initialization
	void Start () {
        crossbow = FindObjectOfType<Crossbow>();
        upgradeTiles = new List<ItemTile>();
        for(int i = 0; i < transform.childCount; i++)
        {
            ItemTile tile = transform.GetChild(i).GetComponent<ItemTile>();
            if (tile != null)
            {
                upgradeTiles.Add(tile);
                tile.onSetItem += SetUpgrade;
                tile.onRemoveItem += RemoveUpgrade;

                currentUpgrades.Add(null);
            }
        }

        leftInfusion.onSetItem += CheckCurrentInfusion;
        rightInfusion.onSetItem += CheckCurrentInfusion;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ToggleCurrentInfusion();
        }
    }

    public void ToggleCurrentInfusion()
    {
        infusionSwitched = !infusionSwitched;
        CheckCurrentInfusion();
    }

    void CheckCurrentInfusion(Item item = null)
    {
        if (infusionSwitched)
        {
            SetInfusion(rightInfusion.item);
        }
        else
        {
            SetInfusion(leftInfusion.item);
        }
    }

    void SetInfusion(Item item)
    {
        if (item == null)
        {
            RemoveInfusion();
        }
        else
        {
            crossbow.properties = item.properties;
            infusionDisplay.SetItem(item);
        }
    }

    void RemoveInfusion(Item item = null)
    {
        crossbow.properties = new List<Property>();
        infusionDisplay.RemoveItem();
    }

    void SetUpgrade(Item item)
    {
        switch(item.upgrade)
        {
            case CrossbowUpgrade.Upgrade.Multishot:
                crossbow.gameObject.AddComponent<Multishot>();
                crossbow.GetComponent<Multishot>().ApplyUpgrade();
                break;
            case CrossbowUpgrade.Upgrade.Scope:
                crossbow.gameObject.AddComponent<Scope>();
                crossbow.GetComponent<Scope>().ApplyUpgrade();
                break;
            case CrossbowUpgrade.Upgrade.Repeater:

                break;
        }
    }

    void RemoveUpgrade(Item item)
    {
        switch (item.upgrade)
        {
            case CrossbowUpgrade.Upgrade.Multishot:
                crossbow.gameObject.GetComponent<Multishot>().RemoveUpgrade();  
                break;
            case CrossbowUpgrade.Upgrade.Scope:
                crossbow.gameObject.GetComponent<Scope>().RemoveUpgrade();
                break;
            case CrossbowUpgrade.Upgrade.Repeater:

                break;
        }
    }
}
