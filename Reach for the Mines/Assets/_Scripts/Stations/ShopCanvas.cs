using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private StationInventory playerInventory = null;
    [SerializeField] private CraftingItem craftingItem = null;
    [SerializeField] private Image purchasablesMenu = null;
    [SerializeField] private Image sellMenu = null;
    [SerializeField] private TMP_Text swapButtonText = null;

    private List<CraftingItem> buyableRecipes = new List<CraftingItem>();
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

    private void SetPurchasables()
    {
        foreach (var item in buyableRecipes)
        {
            Destroy(item.gameObject);
        }
        buyableRecipes.Clear();

        foreach (Craftable craftable in systems.inventoryManager.lockedRecipes)
        {
            CraftingItem newButton = Instantiate(craftingItem, purchasablesMenu.transform);
            buyableRecipes.Add(newButton);
            newButton.FillSlot(craftable);
        }

        foreach (CraftingItem item in buyableRecipes)
        {
            item.CraftingItemButton.onClick.AddListener(delegate { BuyRecipeButton(item); });
        }
    }

    private void SetMenuWhenOpened()
    {
        purchasablesMenu.gameObject.SetActive(true);
        sellMenu.gameObject.SetActive(false);
        swapButtonText.text = "Sell";
        SetPurchasables();
    }

    public void SwapMenu()
    {
        purchasablesMenu.gameObject.SetActive(!purchasablesMenu.gameObject.activeInHierarchy);
        sellMenu.gameObject.SetActive(!sellMenu.gameObject.activeInHierarchy);

        if (purchasablesMenu.gameObject.activeInHierarchy)
        {
            SetPurchasables();
            swapButtonText.text = "Buy";
        }
        else
        {
            playerInventory.PopulateInventory();
            swapButtonText.text = "Sell";
        }

        SetPurchasables();

    }

    private void BuyRecipeButton(CraftingItem item)
    {
        if(!systems.inventoryManager.unlockedRecipes.Contains(item.craftable) &&
            systems.inventoryManager.lockedRecipes.Contains(item.craftable) &&
                systems.statManager.goldAmount >= item.craftable.Two.purchasePrice)
        {
            systems.statManager.goldAmount -= item.craftable.Two.purchasePrice;
            systems.inventoryManager.lockedRecipes.Remove(item.craftable);
            systems.inventoryManager.unlockedRecipes.Add(item.craftable);
            SetPurchasables();
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
