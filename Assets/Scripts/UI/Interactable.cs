using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public string description = "";
    internal bool interactable = true;
    private bool selected = false;

    internal virtual void OnInteract() { }
    internal virtual void OnSelect() { }
    internal virtual void OnDeselect() { }

    private void Update()
    {
        if(selected && Input.GetKeyDown(KeyCode.W))
        {
            OnInteract();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!interactable) return;

        if(collision.transform.tag == "Player")
        {
            selected = true;
            InteractionSelector.Select(transform, description);
            OnSelect();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!selected) return;

        if(collision.transform.tag == "Player")
        {
            selected = false;
            InteractionSelector.Deselect();
            OnDeselect();
        }
    }
}
