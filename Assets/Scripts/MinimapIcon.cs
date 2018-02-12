using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour {
	private SpriteRenderer spriteRend;

	// Use this for initialization
	void Start () {
		spriteRend = GetComponent<SpriteRenderer> ();
		spriteRend.enabled = true;
	}

	public void DisableIcon() {
		spriteRend.enabled = false;
	}
}
