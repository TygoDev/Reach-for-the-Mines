using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText = null;
    [SerializeField] private TMP_Text goldText = null;

    private Systems systems = null;

    void Start()
    {
        systems = Systems.Instance;

        UpdateGoldValue();
        UpdateLevelValue();
        
        if(systems.stateManager.GetGameState() == GameState.Gameplay)
            GetComponent<Canvas>().enabled = true;
        else
            GetComponent<Canvas>().enabled = false;

        Subscribe();
    }

    private void Subscribe()
    {
        systems.stateManager.onGameStateChanged += ToggleCanvas;
        systems.xpGainedEvent += UpdateLevelValue;
        systems.updateCurrencyEvent += UpdateGoldValue;
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= ToggleCanvas;
        systems.xpGainedEvent -= UpdateLevelValue;
        systems.updateCurrencyEvent -= UpdateGoldValue;
    }

    private void UpdateGoldValue()
    {
        goldText.text = systems.statManager.goldAmount.ToString();
    }

    private void UpdateLevelValue()
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
