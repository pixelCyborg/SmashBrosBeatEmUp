using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    int maxHealth;
    int currentHealth;
    List<SpriteRenderer> lives;
    List<Image> uiLives;
    GameObject lifePrefab;

    public bool dynamicPosition;

    public void Start()
    {
        lifePrefab = transform.GetChild(0).gameObject;

        if (GetComponentInParent<Enemy>()) maxHealth = GetComponentInParent<Enemy>().health;
        if (GetComponentInParent<Player>()) maxHealth = Player.Health;

        currentHealth = maxHealth;

        for(int i = 0; i < maxHealth - 1; i++)
        {
            Instantiate(lifePrefab, transform.position + Vector3.up, Quaternion.identity,transform);
        }

        lives = new List<SpriteRenderer>();
        uiLives = new List<Image>();
        for(int i = 0; i < transform.childCount; i++)
        {
            lives.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
            uiLives.Add(transform.GetChild(i).GetComponent<Image>());
        }

        UpdateObjectPositions();
    }

    private void Update()
    {
        if (dynamicPosition)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void UpdateObjectPositions()
    {
        if (!dynamicPosition) return;
        for(int i = 0; i < lives.Count; i++)
        {
            float middle = (float)currentHealth / 2;
            lives[i].transform.localPosition = new Vector3(0.5f * (i - middle) + (0.25f), 1.2f, 0);
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
                    if(lives[i] != null) lives[i].enabled = false;
                    if (uiLives[i] != null) uiLives[i].enabled = false;
                }
                else
                {
                    if (lives[i] != null) lives[i].enabled = true;
                    if (uiLives[i] != null) uiLives[i].enabled = true;
                }
            }
        }
        UpdateObjectPositions();
    }
}
