using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Town : LocationBase, IPointerDownHandler
{
    public string townName;
    public string kingdom;
    public int population;
    public Prosperity prosperity;

    public enum Prosperity
    {
        Wealthy, Thriving, Sustainable, Poor, Starving
    }

    private void OnEnable()
    {
        gameObject.name = townName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        }
        SceneManager.LoadScene("Town", LoadSceneMode.Additive);
    }

    internal override string GetDescription()
    {
        string descriptionString = "";
        //        descriptionString = targetName + "\n";
        descriptionString += kingdom + "\n";
        descriptionString += population + "\n";
        descriptionString += prosperity.ToString() + "\n";

        return descriptionString;
    }

    internal override string GetLabels()
    {
        string labelString = "";
        //       labelString = "Target:\n";
        labelString += "Kingdom:\n";
        labelString += "Population:\n";
        labelString += "Prosperity:\n";

        return labelString;
    }
}
