﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Door : Interactable {
	bool entering = false;
	public string targetScene = "";
	string id;
    private Collider2D playerCol;
    private Collider2D doorCol;

    private void Start()
    {
        doorCol = GetComponent<Collider2D>();
        playerCol = FindObjectOfType<Player>().GetComponent<Collider2D>();
    }

    internal override void OnInteract()
    {
        base.OnInteract();
        if(Physics2D.IsTouching(doorCol, playerCol))
        {
            Enter();
        } 
    }

    public void Enter() {
		entering = true;
		if (string.IsNullOrEmpty (targetScene))
			return;

        InteractionSelector.Deselect();
		SceneManager.LoadScene (targetScene, LoadSceneMode.Additive);
        if (targetScene != "Hub") LightingManager.instance.ToggleDarkness(true);
        SceneManager.UnloadSceneAsync (gameObject.scene);
	}

    /*  -=== Pretty sure this aint being used anymore
	void OnSceneLoad() {
		if (entering) {
			Door[] doors = FindObjectsOfType<Door>();
			for (int i = 0; i < doors.Length; i++) {
				if (doors [i] != this && doors [i].id == id) {
					Player player = FindObjectOfType<Player> ();
					player.transform.position = doors [i].transform.position;
				}
			}
		}
	}
    */
}
