using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateManager", menuName = "Custom/State Manager")]
public class StateManager : ScriptableObject
{

    public GameState currentState = GameState.Gameplay;
    public event Action<GameState> onGameStateChanged = delegate { };

    private void OnEnable()
    {
        onGameStateChanged.Invoke(currentState);
    }

    public void UpdateGameState(GameState state)
    {
        currentState = state;

        switch (currentState)
        {
            case GameState.Gameplay:
                onGameStateChanged.Invoke(currentState);
                break;
            case GameState.Menu:
                onGameStateChanged.Invoke(currentState);
                break;
        }
    }
}

public enum GameState
{
    Gameplay,
    Menu
}
