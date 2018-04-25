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

    public int totalFloors;
    public int currentFloor;
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
        totalFloors = Random.Range(3, 6);

        contractTitle.text = currentContract.targetName;
        dayCounter.text = "Day " + currentDay + " of " + timeframe;
            
        if(TravelMap.mapShown) TravelMap.instance.Toggle();
        MissionTracker.instance.ShowMission(currentContract.targetName, timeframe);
    }

    public void NextDay()
    {
        currentDay++;
        dayCounter.text = "Day " + currentDay + " of " + timeframe;
        if (currentDay > timeframe)
        {
            currentPayout /= 2;
        }

        MissionTracker.instance.UpdateDayCounter(currentDay);
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

    public void NextFloor()
    {
        currentFloor++;
        MissionTracker.instance.UpdateDayCounter(currentDay, currentFloor);
    }

    public void ResetFloors()
    {
        currentFloor = 0;
    }

    public bool IsBossRoom()
    {
        return currentFloor == totalFloors;
    }

    public string GetContractTileset()
    {
        string tileset = "";

        if(currentContract.tileset == Contract.Tileset.Cave)
        {
            tileset = "Cave";
        }
        if(currentContract.tileset == Contract.Tileset.Dungeon)
        {
            tileset = "Green Dungeon";
        }
        if (currentContract.tileset == Contract.Tileset.Slime_Castle)
        {
            tileset = "Slime Castle";
        }

        return tileset;
    }

    public string GetBossRoom()
    {
        string bossName = "";

        if(currentContract.target == Contract.Target.Philosopher_Stone)
        {
            return "Philosopher Stone";
        }

        return bossName;
    }
}
