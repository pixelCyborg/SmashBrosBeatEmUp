using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Door : MonoBehaviour {
	bool entering = false;
	public string targetScene = "";
	string id;

	public void Enter() {
		entering = true;
		if (string.IsNullOrEmpty (targetScene))
			return;

		DontDestroyOnLoad (gameObject);
		SceneManager.LoadScene (targetScene, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync (gameObject.scene);
	}

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
}
