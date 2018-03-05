using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour {
    public static bool paused;
    public static CanvasManager instance;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    private void Start()
    {
        instance = this;
        instance.gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !Inventory.open)
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }
    }

    public static void ShowGameOver()
    {
        instance.gameOverScreen.SetActive(true);
    }

    public static void HideGameOver()
    {
        instance.gameOverScreen.SetActive(false);
    }

    public void ReturnToSurface()
    {
        //Store the scenes to unload first so the for loop doesn't get screwed up
        List<Scene> scenesToUnload = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != gameObject.scene.name) scenesToUnload.Add(scene);
        }

        foreach(Scene scene in scenesToUnload)
        {
            SceneManager.UnloadSceneAsync(scene);
        }

        //Load the hub
        SceneManager.LoadScene("Hub", LoadSceneMode.Additive);
        LightingManager.instance.ToggleDarkness(false);
        HideGameOver();
        Player.instance.Reset();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
