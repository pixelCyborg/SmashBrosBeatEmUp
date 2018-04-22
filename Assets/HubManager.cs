using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Player.instance.transform.position = Vector3.zero;
        LightingManager.instance.ToggleDarkness(false);
	}
}