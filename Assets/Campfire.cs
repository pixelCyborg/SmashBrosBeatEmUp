using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Campfire : Interactable {

	// Use this for initialization
	void Start () {
        Player player = FindObjectOfType<Player>();
        player.transform.position = transform.position + Vector3.up * 2;
	}

    //Return to town
    internal override void OnInteract()
    {
        InteractionSelector.Deselect();

        SceneManager.LoadScene("Hub", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
