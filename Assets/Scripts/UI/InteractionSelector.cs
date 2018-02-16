using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSelector : MonoBehaviour
{
    private static List<Transform> targetQueue;
    private static Transform target;
    private static SpriteRenderer targetSprite;
    private static SpriteRenderer rend;
    private static SpriteGlow.SpriteGlow glow;
    private static Sprite origSprite;
    private static TextMesh interactionText;

    private void Start()
    {
        targetQueue = new List<Transform>();
        rend = GetComponent<SpriteRenderer>();
        glow = GetComponent<SpriteGlow.SpriteGlow>();
        interactionText = GetComponentInChildren<TextMesh>();
        origSprite = rend.sprite;
    }

public static void Select(Transform newTarget, string text = "")
    {
        target = newTarget;
        targetSprite = target.GetComponent<SpriteRenderer>();
        glow.SetMaterialProperties();

        if(text != "")
        {
            interactionText.text = text;
            interactionText.transform.localPosition = Vector3.up * targetSprite.bounds.extents.y * 1f;

        }
    }

    public static void Deselect()
    {
        target = null;
        targetSprite = null;
        rend.sprite = origSprite;
        interactionText.text = "";
        glow.SetMaterialProperties();
    }

    private void Update()
    {
        if (target != null && targetSprite != null)
        {
            rend.sprite = targetSprite.sprite;
            transform.position = target.position;
            transform.localScale = target.localScale;

            if (Input.GetKeyDown(KeyCode.W))
            {
                target.GetComponent<Interactable>().OnInteract();
            }
        }
    }
}

