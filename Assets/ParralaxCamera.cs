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
        Vector3 targetPos = origin + (targetCam.position - targetCamOrigin) * 0.15f;
        targetPos.y = targetCam.position.y;
        transform.position = targetPos;
	}
}
