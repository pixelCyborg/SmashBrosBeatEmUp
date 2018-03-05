using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Healthbar : MonoBehaviour {
    private float shownTime = 2.0f;
    int maxHealth;
    int currentHealth;
    List<SpriteRenderer> lives;
    List<Image> uiLives;
    GameObject lifePrefab;
    CanvasGroup group;

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

        foreach(SpriteRenderer life in lives)
        {
            if (life == null) break;
            Color color = life.color;
            color.a = 0;
            life.color = color;
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
        StartCoroutine(ShowLife());
        UpdateObjectPositions();
    }

    public IEnumerator ShowLife()
    {
        FadeIn(0.2f);
        yield return new WaitForSeconds(shownTime);
        FadeOut(0.2f);
    }

    public void FadeIn(float fadeTime)
    {
        foreach(SpriteRenderer rend in lives)
        {
            rend.DOFade(1.0f, fadeTime);
        }
    }

    private void FadeOut(float fadeTime)
    {
        foreach (SpriteRenderer rend in lives)
        {
            rend.DOFade(0.0f, fadeTime);
        }
    }
}
