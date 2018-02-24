using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocationBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField]
	internal bool interactable = true;
	private bool selected = false;
    public Vector2 location;

	public void OnPointerEnter (PointerEventData eventData)
	{
		//base.OnPointerEnter (eventData);
		if (!interactable)
			return;

		selected = true;
		MapSelector.Select (transform, GetLabels(), GetDescription());
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		//base.OnPointerExit (eventData);
		if (!selected)
			return;

		selected = false;
		MapSelector.Deselect ();
	}

    internal virtual string GetDescription()
    {
        return "";
    } 

    internal virtual string GetLabels()
    {
        return "";
    }
}
