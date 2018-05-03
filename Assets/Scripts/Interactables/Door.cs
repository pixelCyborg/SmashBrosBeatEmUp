using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Door : Interactable {
	bool entering = false;
	public string targetScene = "";
	string id;
    private Collider2D playerCol;
    private Collider2D doorCol;

    public System.Action OnOpen;

    private void Start()
    {
        doorCol = GetComponent<Collider2D>();
        playerCol = FindObjectOfType<Player>().GetComponent<Collider2D>();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
            if (targetScene != "Hub") LightingManager.instance.ToggleDarkness(true);
        };
    }

    internal override void OnInteract()
    {
        base.OnInteract();
        if(Physics2D.IsTouching(doorCol, playerCol))
        {
            try
            {
                GetComponent<AudioSource>().Play();
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }

            Enter();
        } 
    }

    public void Enter() {
		entering = true;
		if (string.IsNullOrEmpty (targetScene))
			return;

        if (targetScene == "Dungeon")
        {
            if (MissionManager.instance.currentFloor >= MissionManager.instance.totalFloors)
            {
                targetScene = MissionManager.instance.GetBossRoom();
            }
            else
            {
                targetScene = MissionManager.instance.GetContractTileset();
            }
        }

        if(OnOpen != null)
        {
            OnOpen();
        }

        InteractionSelector.Deselect(transform);
		SceneManager.LoadScene (targetScene, LoadSceneMode.Additive);
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
