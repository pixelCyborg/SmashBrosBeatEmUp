using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public float speed = 4.0f;
	private Transform target;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
			return;

		transform.position = Vector3.Lerp (transform.position, target.position + offset, Time.deltaTime * speed);
	}
}
