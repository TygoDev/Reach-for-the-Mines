using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveManager : MonoBehaviour
{

    [SerializeField] private float timeInCave = 5;
    [SerializeField] private Vector3 spawnpoint = new Vector3(0,1,0);
    [SerializeField] private TMP_Text timerText = null;

    private Systems systems;
    private Timer caveTime = null;


    void Start()
    {
        systems = Systems.Instance;
        caveTime = new Timer(timeInCave);

        caveTime.OnTimerEnd += TimerEnd;
    }

    private void OnDisable()
    {
        caveTime.OnTimerEnd -= TimerEnd;
    }

    private void Update()
    {
        caveTime.Tick(Time.deltaTime);

        int minutes = Mathf.FloorToInt(caveTime.remainingSeconds / 60F);
        int seconds = Mathf.FloorToInt(caveTime.remainingSeconds - minutes * 60);

        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = $"Time Left:\n{niceTime}";
    }

    void TimerEnd()
    {
        systems.spawnpoint = spawnpoint;

        SceneManager.LoadScene("Hub World");
    }
}
