using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActivator : MonoBehaviour {
	// Use this for initialization
	void Awake () {
        GetComponent<Canvas>().enabled = true;
	}
}
