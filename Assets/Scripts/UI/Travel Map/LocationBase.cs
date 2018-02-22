using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocationBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
	[SerializeField]
	public string description = "";
	internal bool interactable = true;
	private bool selected = false;

	public void OnPointerEnter (PointerEventData eventData)
	{
		//base.OnPointerEnter (eventData);
		if (!interactable)
			return;

		selected = true;
		MapSelector.Select (transform, description);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		//base.OnPointerExit (eventData);
		if (!selected)
			return;

		selected = false;
		MapSelector.Deselect ();
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		//base.OnPointerClick (eventData);
	}
}
