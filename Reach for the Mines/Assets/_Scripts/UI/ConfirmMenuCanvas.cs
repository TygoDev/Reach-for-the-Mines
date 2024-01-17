using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConfirmMenuCanvas : MonoBehaviour
{
    [SerializeField] private Button cancel = null;
    [SerializeField] private Canvas canvas = null;

    private Systems systems = null;

    private void Start()
    {

        systems = Systems.Instance;

        cancel.onClick.AddListener(Cancel);
    }

    private void Cancel()
    {
        systems.stateManager.UpdateGameState(GameState.Gameplay);
        canvas.enabled = false;
    }
}
