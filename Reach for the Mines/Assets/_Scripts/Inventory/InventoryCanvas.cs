using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private Image inventory = null;
    [SerializeField] private List<InventoryItem> inventoryItemSlots = new List<InventoryItem>();

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
        inventory.gameObject.SetActive(true);
        PopulateInventory();
    }

    public void OnInventoryClose()
    {
        inventory.gameObject.SetActive(false);
    }

    private void PopulateInventory()
    {
        for (int i = 0; i < systems.inventoryManager.items.Count; i++)
        {
            inventoryItemSlots[i].ClearSlot();
            inventoryItemSlots[i].FillSlot(systems.inventoryManager.items[i]);
        }
    }
}
