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

    public Transform[] _targetQueue;

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
        targetQueue.Add(target);
        targetSprite = target.GetComponent<SpriteRenderer>();
        glow.SetMaterialProperties();

        if(text != "")
        {
            interactionText.text = text;
            interactionText.transform.localPosition = Vector3.up * targetSprite.bounds.extents.y * 1f;

        }
    }

    public static void Deselect(Transform _target)
    {
        if(_target != null && targetQueue.Count > 0)
        {
            if(target.GetInstanceID() == _target.GetInstanceID())
            {
                if (targetQueue.Count <= 1)
                {
                    targetQueue.RemoveAt(0);
                    Deselect();
                }
                else
                {
                    targetQueue.RemoveAt(targetQueue.Count - 1);
                    Select(targetQueue[targetQueue.Count - 1], targetQueue[targetQueue.Count - 1].GetComponent<Interactable>().description);
                }
            }
            else
            {
                for(int i = 0; i < targetQueue.Count; i++)
                {
                    if (_target.GetInstanceID() == targetQueue[i].GetInstanceID()) targetQueue.RemoveAt(i);
                    return;
                }
                Deselect();
            }
        }
        else
        {
            Deselect();
        }
    }

    private static void Deselect()
    {
        target = null;
        targetSprite = null;
        rend.sprite = origSprite;
        interactionText.text = "";
        glow.SetMaterialProperties();
    }

    private void Update()
    {
        _targetQueue = targetQueue.ToArray();

        if (target != null && targetSprite != null)
        {
            rend.sprite = targetSprite.sprite;
            transform.position = target.position;
            transform.localScale = target.localScale;

            if (!CanvasManager.paused && Input.GetKeyDown(KeyCode.W))
            {
                target.GetComponent<Interactable>().OnInteract();
            }
        }
    }
}

