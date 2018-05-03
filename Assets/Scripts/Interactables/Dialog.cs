using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : Interactable {
    public float dialogTime = 1.0f;
    public string dialogString;
    [HideInInspector]
    public SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    internal override void OnInteract()
    {
        base.OnInteract();
        DialogManager.instance.SetDialog(this);
    }

    internal override void OnDeselect()
    {
        base.OnDeselect();
        DialogManager.instance.HideDialog();
    }
}
