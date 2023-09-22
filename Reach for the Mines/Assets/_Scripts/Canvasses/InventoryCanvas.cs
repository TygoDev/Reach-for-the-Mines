using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private Systems systems = default;

    private void Start()
    {
        systems = Systems.Instance;
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.openInventoryEvent += OnInventoryOpen;
        systems.inputManager.unPauseEvent += OnInventoryClose;
    }

    private void OnDisable()
    {
        systems.inputManager.openInventoryEvent -= OnInventoryOpen;
        systems.inputManager.unPauseEvent -= OnInventoryClose;
    }

    public void OnInventoryOpen()
    {
        GetComponent<Canvas>().enabled = true;
        PopulateInventory();
    }

    public void OnInventoryClose()
    {
        GetComponent<Canvas>().enabled = false;
    }

    private void PopulateInventory()
    {
        for (int i = 0; i < systems.inventoryManager.items.Count; i++)
        {
            inventoryItems[i].ClearSlot();
            inventoryItems[i].FillSlot(systems.inventoryManager.items.ElementAt(i));
        }
    }
}
