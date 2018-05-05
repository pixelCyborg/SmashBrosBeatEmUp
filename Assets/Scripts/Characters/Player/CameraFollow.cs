using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private static bool following = true;
	public float speed = 4.0f;
    public float zoomSpeed = 2.0f;
    public float maxZoom = 10.0f;
    public float minZoom = 4.0f;
    private static float MaxZoom;
    private static float MinZoom;
	private static Transform target;
    private static Transform defaultTarget;
	private Vector3 offset = new Vector3(0, 0, -5);
    private static float defaultZoom;
    private static float targetZoom;
    private static Camera cam;
    
    public static BoxCollider2D focusedRoom;

	// Use this for initialization
	void Start () {
        MaxZoom = maxZoom;
        MinZoom = minZoom;
        cam = GetComponentInChildren<Camera>();
        defaultZoom = cam.orthographicSize;
        targetZoom = defaultZoom;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
        defaultTarget = target;
	}
	
    public static void Focus(Transform _target)
    {
        following = false;
        target = _target;
    }

    public static void Unfocus()
    {
        target = defaultTarget;
    }

    public static void FocusRoom(BoxCollider2D col)
    {
        if (col == null) return;
        focusedRoom = col;

        Vector2 aspect = new Vector2(Screen.width, Screen.height).normalized;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = col.transform.localScale.x / col.transform.localScale.y;


        if (screenRatio >= targetRatio)
        {
            targetZoom = col.transform.localScale.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            targetZoom = col.transform.localScale.y / 2 * differenceInSize;
        }

        targetZoom *= 1.2f;
        if (targetZoom > MaxZoom) targetZoom = MaxZoom;
        else if (targetZoom < MinZoom) targetZoom = MinZoom;

        if (targetZoom > MaxZoom * 1.5f) target = null;

        target = col.transform;
    }

    public static void UnFocusRoom(BoxCollider2D col)
    {
        if (focusedRoom == col)
        {
            focusedRoom = null;
            targetZoom = defaultZoom;
            target = defaultTarget;
        }
    }

	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetZoom = defaultZoom;
            return;
        }

        if(!following) {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
            if (target.gameObject == defaultTarget.gameObject && Vector3.Distance(transform.position, target.position) < 0.1f) following = true;
            return;
        }

        Vector3 reticlePos = cam.ScreenToWorldPoint(Input.mousePosition);
        reticlePos.z = 0;

        //transform.position = Vector3.Lerp (transform.position, (target.position + defaultTarget.position + reticlePos * 0.5f)/2.5f + offset, Time.deltaTime * speed);
        transform.position = (target.position + defaultTarget.position + reticlePos * 0.5f)/ 2.5f + offset;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
	}
}
