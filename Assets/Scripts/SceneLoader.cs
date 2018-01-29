using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public string targetScene;

	// Use this for initialization
	void Start () {
        if (!string.IsNullOrEmpty(targetScene) && SceneManager.sceneCount <= 1)
        {
            SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        }

        Destroy(gameObject);
	}
}
