using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightSource : MonoBehaviour {
    public Color tint = Color.clear;
    public Transform origin;
    public float radius = 5.0f;
    public Strength strength = Strength.Full;
    public GameObject dimLight;
    public GameObject regularLight;
    public GameObject fullLight;
    public SpriteRenderer tintSprite;


    public enum Strength
    {
        Dim, Regular, Full
    }

    public void Initialize()
    {
        switch(strength)
        {
            case Strength.Dim:
                fullLight.SetActive(false);
                regularLight.SetActive(false);
                dimLight.transform.localScale = Vector3.one * radius;
                tintSprite.transform.localScale = Vector3.one * radius;
                break;

            case Strength.Regular:
                fullLight.SetActive(false);
                regularLight.transform.localScale = Vector3.one * radius;
                dimLight.transform.localScale = Vector3.one * radius * 1.5f;
                tintSprite.transform.localScale = Vector3.one * radius * 1.5f;
                break;

            case Strength.Full:
                fullLight.transform.localScale = Vector3.one * radius;
                regularLight.transform.localScale = Vector3.one * radius * 1.5f;
                dimLight.transform.localScale = Vector3.one * radius * 2.0f;
                tintSprite.transform.localScale = Vector3.one * radius * 2.0f;
                break;
        }

        if(tint != Color.clear && tint != Color.black)
        {
            tint.a = 0.05f;
            tintSprite.color = tint;
        }
        else
        {
            tintSprite.enabled = false;
        }
    }

    public void Fade()
    {
        StartCoroutine(_Fade());
    }

    IEnumerator _Fade()
    {
        yield return new WaitForSeconds(0.33f);
        Destroy(fullLight);
        transform.localScale *= 0.66f;
        yield return new WaitForSeconds(0.33f);
        Destroy(regularLight);
        transform.localScale *= 0.66f;
        yield return new WaitForSeconds(0.33f);
        Destroy(dimLight);
    }

    private void Update()
    {
        if (origin != null) transform.position = origin.position;
        else Destroy(gameObject);
    }
}
