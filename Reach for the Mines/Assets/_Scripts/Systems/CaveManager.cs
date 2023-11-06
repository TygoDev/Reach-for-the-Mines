using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveManager : MonoBehaviour
{

    [SerializeField] private float timeInCave = 5;

    private Timer caveTime = null;


    void Start()
    {
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
        SceneManager.LoadScene("Hub World");
    }
}
