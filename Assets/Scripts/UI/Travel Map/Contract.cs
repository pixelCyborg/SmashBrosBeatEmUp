using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Contract : LocationBase, IPointerDownHandler {
    public string targetName;
    public int payment;
    public int timeframe;
    public Difficulty difficulty;
    public Tileset tileset;

    public enum Tileset
    {
        Dungeon, Cave
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
        Debug.Log("Starting mission!");
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
