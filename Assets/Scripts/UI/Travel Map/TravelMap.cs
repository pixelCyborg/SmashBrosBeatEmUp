using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelMap : MonoBehaviour {
	private List<Transform> locations;
	private CanvasGroup groupComponent;
	bool mapShown = true;

	void Start() {
		groupComponent = GetComponent<CanvasGroup> ();
		mapShown = true;
		Toggle ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.M)) {
			Toggle ();
		}
	}

	void Toggle() {
		if (mapShown) {
			mapShown = false;
			groupComponent.alpha = 0;
			groupComponent.interactable = false;
			groupComponent.blocksRaycasts = false;
		} else {
			mapShown = false;
			groupComponent.alpha = 1;
			groupComponent.interactable = true;
			groupComponent.blocksRaycasts = true;
		}
	}

	//Map Generation
}
