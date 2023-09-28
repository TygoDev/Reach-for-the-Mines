using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [SerializeField] private Image purchasablesMenu = null;
    [SerializeField] private Image sellMenu = null;
    [SerializeField] private TMP_Text swapButtonText = null;

    private Systems systems = default;

    private void Start()
    {
        systems = Systems.Instance;
        Initialize();
    }

    private void Initialize()
    {
        systems.stateManager.onGameStateChanged += OnGameStateChange;
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    public void SwapMenu()
    {
        if (purchasablesMenu.gameObject.activeInHierarchy)
            swapButtonText.text = "Buy";
        else
            swapButtonText.text = "Sell";

        purchasablesMenu.gameObject.SetActive(!purchasablesMenu.gameObject.activeInHierarchy);
        sellMenu.gameObject.SetActive(!sellMenu.gameObject.activeInHierarchy);
        PopulateInventory();
    }

    public void SellButton(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty == false)
        {
            int goldToAdd = inventoryItem.itemStack.quantity * inventoryItem.itemStack.item.value;
            systems.statManager.goldAmount += goldToAdd;
            systems.inventoryManager.Remove(inventoryItem.itemStack);
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

    public void OnGameStateChange(GameState state)
    {
        if (state == GameState.Menu && GetComponent<Canvas>().enabled == true)
        {
            purchasablesMenu.gameObject.SetActive(true);
            sellMenu.gameObject.SetActive(false);
            swapButtonText.text = "Sell";
        }
    }
}
