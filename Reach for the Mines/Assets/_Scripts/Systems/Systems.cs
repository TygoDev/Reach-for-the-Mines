using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Systems : MonoBehaviour
{
    private static Systems instance;
    public static Systems Instance { get { return instance; } }

    public StateManager stateManager;
    public InventoryManager inventoryManager;
    public InputManager inputManager;
    public StatManager statManager;

    public UnityAction xpGainedEvent = delegate { };
    public UnityAction updateCurrencyEvent = delegate { };
    public UnityAction<Item> stationClickedEvent = delegate { };

    public Vector3 spawnpoint = new Vector3(0,1,0);
    public bool tutorialComplete = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            stateManager.UpdateGameState(GameState.Gameplay);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}