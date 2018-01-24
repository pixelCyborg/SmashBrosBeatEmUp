using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Door : MonoBehaviour {
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

    private void Update()
    {
        if(Physics2D.IsTouching(doorCol, playerCol) && Input.GetKeyDown(KeyCode.W))
        {
            Enter();
        } 
    }

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
