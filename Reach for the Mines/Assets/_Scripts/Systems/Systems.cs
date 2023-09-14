using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Systems : MonoBehaviour
{
    private static Systems instance;
    public static Systems Instance { get { return instance; } }

    public StateManager stateManager;

    private void Start()
    {
        stateManager.onGameStateChanged += Test;
    }

    private void Test(GameState obj)
    {
        Debug.Log(obj);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
