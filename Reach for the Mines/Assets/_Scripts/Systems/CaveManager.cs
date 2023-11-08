using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveManager : MonoBehaviour
{

    [SerializeField] private float timeInCave = 5;
    [SerializeField] private Vector3 spawnpoint = new Vector3(0,1,0);

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
    }

    void TimerEnd()
    {
        systems.spawnpoint = spawnpoint;

        SceneManager.LoadScene("Hub World");
    }
}
