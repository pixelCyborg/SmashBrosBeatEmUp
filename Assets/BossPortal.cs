using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortal : Interactable {
    //Return to town
    internal override void OnInteract()
    {
        InteractionSelector.Deselect(transform);

        LightingManager.instance.ToggleDarkness(false);
        SceneManager.LoadScene("Hub", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
