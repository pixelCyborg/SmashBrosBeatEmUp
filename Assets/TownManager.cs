using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LightingManager.instance.ToggleDarkness(false);
        Player.instance.transform.position = Vector3.zero;
        PlayTownTheme();
	}

    void PlayTownTheme()
    {
        GetComponent<AudioSource>().Play();
    }
}
