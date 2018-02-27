using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MissionTracker : MonoBehaviour {
    public static MissionTracker instance;
    private CanvasGroup group;
    public Text contractTitle;
    public Text dayCounter;
    public float revealTime;

    private string missionTitle;
    private int missionTimeframe;
    private int currentDay;

    private void Start()
    {
        instance = this;
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }

    public void ShowMission(string title, int timeframe)
    {
        currentDay = 1;
        missionTitle = title;
        missionTimeframe = timeframe;
        contractTitle.text = title;
        dayCounter.text = "Day 1" + " Of " + timeframe;
        ShowMission();
    }

    public void UpdateDayCounter(int days)
    {
        currentDay = days;
        dayCounter.text = "Day " + days + " Of " + missionTimeframe;
        ShowMission();
    }

    public void ShowMission()
    {
        StartCoroutine(_ShowMission());
    }

    private IEnumerator _ShowMission()
    {
        group.DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(revealTime);
        FadeOut();
    }

    private void FadeOut()
    {
        group.DOFade(0.0f, 0.5f);
    }
}