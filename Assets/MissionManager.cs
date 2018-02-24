using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {
    public static MissionManager instance;
    Contract currentContract;

    public Text contractTitle;
    public Text dayCounter;

    public bool objectiveComplete;
    public int currentPayout;
    public int currentDay;
    public int timeframe;

    private int partyFunds;

    private void Start()
    {
        instance = this;
    }

    public void StartMission(Contract contract)
    {
        currentContract = contract;
        objectiveComplete = false;
        timeframe = contract.timeframe;
        currentPayout = contract.payment;
        currentDay = 1;

        contractTitle.text = currentContract.targetName;
        dayCounter.text = "Day " + currentDay + " out of " + timeframe;
    }

    public void NextDay()
    {
        currentDay++;
        dayCounter.text = "Day " + currentDay + "out of " + timeframe;
        if (currentDay > timeframe)
        {
            currentPayout /= 2;
        }
    }

    public void AbandonMission()
    {
        currentPayout = 1;
        currentContract = null;
    }

    public void CompleteMission()
    {
        partyFunds += currentPayout;
        currentContract = null;
    }
}
