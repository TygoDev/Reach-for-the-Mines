using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    public event Action OnDestroyed;

    [SerializeField] private Canvas menuToTrigger = null;

    private Systems systems = default;

    private void Start()
    {
        systems = Systems.Instance;
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(menuToTrigger == null)
            menuToTrigger = GetComponentInChildren<Canvas>();

        menuToTrigger.enabled = false;

        if(systems!=null)
        systems.inputManager.unPauseEvent += OnInventoryClose;
    }

    public void ToogleMenu(bool value)
    {

        if (value)
        {
            EventBus<StationInteractedEvent>.Publish(new StationInteractedEvent(gameObject));
            systems.stateManager.UpdateGameState(GameState.Menu);
        }
        else
        {
            systems.stateManager.UpdateGameState(GameState.Gameplay);
        }

        menuToTrigger.enabled = value;
    }


    private void OnDisable()
    {
        systems.inputManager.unPauseEvent -= OnInventoryClose;
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
