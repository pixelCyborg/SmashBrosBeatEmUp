using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    BoxCollider2D roomCollider;
    bool initialized;

    void Start()
    {
        roomCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        initialized = true;
    }

    //Removing room focus for now
    /*
    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        if (initialized && CameraFollow.focusedRoom != null) return;
        CameraFollow.FocusRoom(roomCollider);
    }
    
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        if (CameraFollow.focusedRoom != null) return;
        CameraFollow.FocusRoom(roomCollider);
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        CameraFollow.UnFocusRoom(roomCollider);
    }
    */
}
