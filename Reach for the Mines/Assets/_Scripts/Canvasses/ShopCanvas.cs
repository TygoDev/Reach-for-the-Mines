using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    private Systems systems = default;

    private void Start()
    {
        systems = Systems.Instance;
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.unPauseEvent += OnInventoryClose;
    }

    private void OnDisable()
    {
        systems.inputManager.unPauseEvent -= OnInventoryClose;
    }

    public void OnInventoryClose()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
