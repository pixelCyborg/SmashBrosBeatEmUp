using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Contract : LocationBase, IPointerDownHandler {
    public string targetName;
    public int payment;
    public int timeframe;
    public Difficulty difficulty;
    public Tileset tileset;
    public Target target;
    public int floors;

    public enum Tileset
    {
        Dungeon, Cave, Slime_Castle
    }

    public enum Target
    {
        Philosopher_Stone
    }

    public enum Difficulty
    {
        Effortless, Easy, Regular, Grueling, Hopeless  
    };

    private void OnEnable()
    {
        gameObject.name = targetName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != "Player")
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
        SceneManager.LoadScene("Hub", LoadSceneMode.Additive);

        CanvasManager.instance.audioHandler.PlayMapSelect();
        MissionManager.instance.StartMission(this);
    }

    internal override string GetDescription()
    {
        string descriptionString = "";
//        descriptionString = targetName + "\n";
        descriptionString += payment + "\n";
        descriptionString += timeframe + "\n";
        descriptionString += tileset.ToString() + "\n";
        descriptionString += difficulty.ToString();

        return descriptionString;
    }

    internal override string GetLabels()
    {
        string labelString = "";
 //       labelString = "Target:\n";
        labelString += "Payout:\n";
        labelString += "Timeframe:\n";
        labelString += "Location:\n";
        labelString += "Difficulty:\n";

        return labelString;
    }
}
