using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Interactable {
    public List<ItemDrop> dropTable = new List<ItemDrop>();
    public int minDrops;
    public int maxDrops;

    internal override void OnInteract()
    {
        base.OnInteract();
        for(int i = Random.Range(minDrops, maxDrops) - 1; i >= 0; i--)
        {
            SpawnItem(RollItem());
        }

    }

    void SpawnItem(GameObject item)
    {
        GameObject spawnedItem = Instantiate(item, transform.position, item.transform.rotation);
        Rigidbody2D body = spawnedItem.GetComponent<Rigidbody2D>();
        if (body == null) return;

        body.AddForce(new Vector2(Random.Range(-3, 3), 10.0f), ForceMode2D.Impulse);
        body.AddTorque(Random.Range(-45, 45));
    }


    private GameObject RollItem()
    {
        dropTable.Sort();

        int maxRoll = 0;
        for (int i = 0; i < dropTable.Count; i++)
        {
            maxRoll += dropTable[i].dropChance;
        }

        int roll = Random.Range(0, maxRoll);
        int index = 0;

        for (int i = 0; i < dropTable.Count; i++)
        {
            if (roll < dropTable[i].dropChance + index) return dropTable[i].item;
            index += dropTable[i].dropChance;
        }

        return null;
    }
}
