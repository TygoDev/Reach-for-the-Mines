using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private StationInventory playerInventory = null;

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

        playerInventory.itemStacks = systems.inventoryManager.items;

        foreach (InventoryItem item in playerInventory.inventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { SellButton(item); });
        }
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    private void SetMenuWhenOpened()
    {
        purchasablesMenu.gameObject.SetActive(true);
        sellMenu.gameObject.SetActive(false);
        swapButtonText.text = "Sell";
    }

    public void SwapMenu()
    {
        purchasablesMenu.gameObject.SetActive(!purchasablesMenu.gameObject.activeInHierarchy);
        sellMenu.gameObject.SetActive(!sellMenu.gameObject.activeInHierarchy);

        if (purchasablesMenu.gameObject.activeInHierarchy)
        {
            playerInventory.PopulateInventory();
            swapButtonText.text = "Buy";
        }
        else
        {
            swapButtonText.text = "Sell";
        }

    }

    public void SellButton(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty == false)
        {
            int goldToAdd = inventoryItem.itemStack.Two * inventoryItem.itemStack.One.value;
            systems.statManager.goldAmount += goldToAdd;
            systems.itemSoldEvent.Invoke();
            systems.inventoryManager.RemoveStack(inventoryItem.itemStack);
            playerInventory.PopulateInventory();
        }
    }

    // -------------- EVENT LISTENERS -------------- 

    public void OnGameStateChange(GameState state)
    {
        if (state == GameState.Menu && GetComponent<Canvas>().enabled == true)
        {
            SetMenuWhenOpened();
        }
    }
}
