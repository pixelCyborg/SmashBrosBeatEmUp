using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : Interactable {
    public List<ItemDrop> dropTable = new List<ItemDrop>();
    public int minDrops;
    public int maxDrops;

    public int minGoldDrop;
    public int maxGoldDrop;

    private GameObject coinPrefab;

    private bool opened = false;

    internal override void OnInteract()
    {
        base.OnInteract();

        if (opened) return;
        opened = true;
        interactable = false;
        InteractionSelector.Deselect();
        for(int i = Random.Range(minDrops, maxDrops) - 1; i >= 0; i--)
        {
            SpawnItem(RollItem());
        }

        if (coinPrefab == null) coinPrefab = (GameObject)Resources.Load("Coin");
        DropCoins();

    }

    void DropCoins()
    {
        int coins = Random.Range(minGoldDrop, maxGoldDrop + 1);
        for (int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
            Rigidbody2D coinBody = coin.GetComponent<Rigidbody2D>();
            coinBody.AddForce(new Vector2(Random.Range(-2, 2), 7.0f), ForceMode2D.Impulse);
        }
    }

    void SpawnItem(GameObject item)
    {
        GameObject spawnedItem = Instantiate(item, transform.position, item.transform.rotation);
        Rigidbody2D body = spawnedItem.GetComponent<Rigidbody2D>();
        if (body == null) return;

        body.AddForce(new Vector2(Random.Range(-2, 2), 7.0f), ForceMode2D.Impulse);
        body.AddTorque(Random.Range(-45, 45));

        StartCoroutine(DelayedItemEnable(body, spawnedItem.GetComponent<Collectable>()));
    }

    IEnumerator DelayedItemEnable(Rigidbody2D body, Collectable item)
    {
        item.enabled = false;
        while(body.velocity.y > 0)
        {
            yield return null;
        }
        item.enabled = true;
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
