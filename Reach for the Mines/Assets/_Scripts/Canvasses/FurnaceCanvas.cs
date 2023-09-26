using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FurnaceCanvas : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [SerializeField] private InventoryItem fuelSlot = null;
    [SerializeField] private InventoryItem inputSlot = null;
    [SerializeField] private InventoryItem outputSlot = null;
    [SerializeField] private InventoryItem slotToFill = null;

    [SerializeField] private Image furnaceMenu = null;
    [SerializeField] private Image inventoryMenu = null;

    private Systems systems = default;

    private void Start()
    {
        systems = Systems.Instance;
        Subscribe();
        GetComponent<Canvas>().enabled = false;
    }

    private void Subscribe()
    {
        systems.inputManager.unPauseEvent += OnInventoryClose;
        systems.stateManager.onGameStateChanged += OnGameStateChange;
    }

    private void OnDisable()
    {
        systems.inputManager.unPauseEvent -= OnInventoryClose;
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    public void SwapMenu()
    {
        if (furnaceMenu.gameObject.activeInHierarchy)
        {
            furnaceMenu.gameObject.SetActive(false);
            inventoryMenu.gameObject.SetActive(true);
            PopulateInventory();
        }
        else
        {
            furnaceMenu.gameObject.SetActive(true);
            inventoryMenu.gameObject.SetActive(false);
        }
    }

    public void FuranceSlot(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty)
        {
            slotToFill = inventoryItem;
            SwapMenu();
        }
        else
        {
            TakeOutput(inventoryItem);
        }

    }

    public void FillSlot(InventoryItem inventoryItem)
    {
        if (!inventoryItem.empty && slotToFill.empty)
        {
            if(inventoryItem.item.itemType == ItemType.Smeltable && slotToFill == inputSlot)
            {
                SwapMenu();
                inputSlot.FillSlot(inventoryItem.itemStack);
                systems.inventoryManager.Remove(inventoryItem.itemStack);
                PopulateInventory();
                slotToFill = null;
            }
            else if (inventoryItem.item.itemType == ItemType.Fuel && slotToFill == fuelSlot)
            {
                SwapMenu();
                fuelSlot.FillSlot(inventoryItem.itemStack);
                systems.inventoryManager.Remove(inventoryItem.itemStack);
                PopulateInventory();
                slotToFill = null;
            }
        }
    }

    public void TakeOutput(InventoryItem inventoryItem)
    {
        if (!inventoryItem.empty)
        {
            for (int i = 0; i < inventoryItem.itemStack.quantity; i++)
            {
                systems.inventoryManager.Add(inventoryItem.itemStack.item, null);
            }
            inventoryItem.ClearSlot();
            PopulateInventory();
        }
    }

    private void PopulateInventory()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].ClearSlot();
            if (systems.inventoryManager.items.Count > i)
                inventoryItems[i].FillSlot(systems.inventoryManager.items.ElementAt(i));
        }
    }


    // EVENT LISTENERS

    public void OnInventoryClose()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void OnGameStateChange(GameState state)
    {
        if (state == GameState.Menu && GetComponent<Canvas>().enabled == true)
        {
            furnaceMenu.gameObject.SetActive(true);
            inventoryMenu.gameObject.SetActive(false);
        }
    }
}
