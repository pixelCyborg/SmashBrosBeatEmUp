using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
    private Enemy brain;

	// Use this for initialization
	void Start () {
        brain = GetComponentInParent<Enemy>();
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (brain == null) return;
        if(collision.tag == "Player")
        {
            brain.target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (brain == null) return;
        if(collision.tag == "Player")
        {
            brain.target = null;
        }
    }
}
