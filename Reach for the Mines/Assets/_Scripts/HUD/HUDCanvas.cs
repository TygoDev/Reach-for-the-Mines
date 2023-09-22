using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText = null;

    private Systems systems = null;

    void Start()
    {
        systems = Systems.Instance;
        levelText.text = systems.statManager.level.ToString();

        Subscribe();
    }

    private void Subscribe()
    {
        systems.stateManager.onGameStateChanged += ToggleCanvas;
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= ToggleCanvas;
    }

    private void Update()
    {
        levelText.text = systems.statManager.level.ToString();
    }

    private void ToggleCanvas(GameState state)
    {
        if (state == GameState.Gameplay)
            GetComponent<Canvas>().enabled = true;
        else if (state == GameState.Menu)
            GetComponent<Canvas>().enabled = false;
    }


}
