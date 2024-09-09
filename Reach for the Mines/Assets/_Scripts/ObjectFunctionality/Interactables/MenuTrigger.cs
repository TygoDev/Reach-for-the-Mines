using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    public event Action OnDestroyed;

    [SerializeField] private Canvas menuToTrigger = null;

    private void OnEnable()
    {
        if (menuToTrigger == null)
            menuToTrigger = GetComponentInChildren<Canvas>();

        menuToTrigger.enabled = false;

        if (Systems.Instance != null)
            Systems.Instance.inputManager.unPauseEvent += OnInventoryClose;
    }

    private void OnDisable()
    {
        Systems.Instance.inputManager.unPauseEvent -= OnInventoryClose;
    }

    public void ToggleMenu(bool value)
    {

        if (value)
        {
            EventBus<StationInteractedEvent>.Publish(new StationInteractedEvent(gameObject));
            Systems.Instance.stateManager.UpdateGameState(GameState.Menu);
        }
        else
        {
            Systems.Instance.stateManager.UpdateGameState(GameState.Gameplay);
        }

        menuToTrigger.enabled = value;
    }


    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }

    // EVENT LISTENERS

    public void OnInventoryClose()
    {
        menuToTrigger.enabled = false;
    }
}
