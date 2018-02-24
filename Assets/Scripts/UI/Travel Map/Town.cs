using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : LocationBase {
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
