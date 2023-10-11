using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    private Canvas menuToTrigger = null;
    private Systems systems = default;

    private void Start()
    {
        systems = Systems.Instance;
        Initialize();
    }
    private void Initialize()
    {
        menuToTrigger = GetComponentInChildren<Canvas>();
        menuToTrigger.enabled = false;
        systems.inputManager.unPauseEvent += OnInventoryClose;
    }

    public void ToogleMenu(bool value)
    {
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
