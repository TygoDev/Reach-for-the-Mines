using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
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
            systems.stateManager.UpdateGameState(GameState.Menu);
        else
            systems.stateManager.UpdateGameState(GameState.Gameplay);

        menuToTrigger.enabled = value;
    }


    private void OnDisable()
    {
        systems.inputManager.unPauseEvent -= OnInventoryClose;
    }

    // EVENT LISTENERS

    public void OnInventoryClose()
    {
        menuToTrigger.enabled = false;
    }
}
