using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionSelector : MonoBehaviour
{
    private static List<Transform> targetQueue;
    private static Transform target;
    private static SpriteRenderer targetSprite;
    private static SpriteRenderer rend;
    private static SpriteGlow.SpriteGlow glow;
    private static Sprite origSprite;
    private static TextMesh interactionText;
    private static InteractionSelector instance;

    public Transform[] _targetQueue;
    public InteractionAudioHandler audioHandler;

    private void Start()
    {
        instance = this;
        audioHandler.Initialize(GetComponent<AudioSource>());
        targetQueue = new List<Transform>();
        rend = GetComponent<SpriteRenderer>();
        glow = GetComponent<SpriteGlow.SpriteGlow>();
        interactionText = GetComponentInChildren<TextMesh>();
        origSprite = rend.sprite;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {

    }

public static void Select(Transform newTarget, string text = "")
    {
        instance.audioHandler.PlaySelect();
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
            try
            {
                if (target.GetInstanceID() == _target.GetInstanceID())
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
                    for (int i = 0; i < targetQueue.Count; i++)
                    {
                        if (_target.GetInstanceID() == targetQueue[i].GetInstanceID()) targetQueue.RemoveAt(i);
                        return;
                    }
                    Deselect();
                }
            }
            catch(System.Exception e)
            {
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
        instance.audioHandler.PlayDeselect();
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

