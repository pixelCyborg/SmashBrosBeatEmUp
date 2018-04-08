using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxCamera : MonoBehaviour {
    public Transform targetCam;
    Vector3 targetCamOrigin;
    Vector3 origin;

	// Use this for initialization
	void Start () {
        targetCam = Camera.main.transform;
        origin = transform.position;
        targetCamOrigin = targetCam.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = origin + (targetCam.position - targetCamOrigin) * 0.15f;
	}
}
