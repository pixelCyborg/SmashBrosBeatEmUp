using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {
    int maxHealth;
    int currentHealth;
    List<SpriteRenderer> lives;
    GameObject lifePrefab;

    public void Start()
    {
        lifePrefab = transform.GetChild(0).gameObject;
        maxHealth = GetComponentInParent<NPC>().health;
        currentHealth = maxHealth;

        for(int i = 0; i < maxHealth - 1; i++)
        {
            Instantiate(lifePrefab, transform.position + Vector3.up, Quaternion.identity,transform);
        }

        lives = new List<SpriteRenderer>();
        for(int i = 0; i < transform.childCount; i++)
        {
            lives.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }

        UpdateObjectPositions();
    }

    void UpdateObjectPositions()
    {
        for(int i = 0; i < lives.Count; i++)
        {
            float middle = (float)currentHealth / 2;
            lives[i].transform.localPosition = new Vector3(0.5f * (i - middle) + (maxHealth % 2 == 0 ? 0 : 0.25f), 1.2f, 0);
        }
    }

    public void SetLifeCount(int count)
    {
        currentHealth = count;
        if(transform.childCount > 0)
        {
            for(int i = 0; i < lives.Count; i++)
            {
                if(i > count - 1)
                {
                    lives[i].enabled = false;
                }
            }
        }
        UpdateObjectPositions();
    }
}
