using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private StationInventory playerInventory = null;
    [SerializeField] private PurchasableItem craftingItem = null;
    [SerializeField] private Image purchasablesMenu = null;
    [SerializeField] private Image sellMenu = null;
    [SerializeField] private TMP_Text swapButtonText = null;
    [SerializeField] private List<Item> buyableItems = new List<Item>();

    private List<PurchasableItem> buyables = new List<PurchasableItem>();
    private Systems systems = default;
    private bool setToRecipes = true;

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

        SetRecipes();
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    private void SetRecipes()
    {
        foreach (var item in buyables)
        {
            Destroy(item.gameObject);
        }
        buyables.Clear();

        foreach (Craftable craftable in systems.inventoryManager.lockedRecipes)
        {
            PurchasableItem newButton = Instantiate(craftingItem, purchasablesMenu.transform);
            buyables.Add(newButton);
            newButton.FillSlot(craftable, null);
        }

        foreach (PurchasableItem item in buyables)
        {
            item.CraftingItemButton.onClick.AddListener(delegate { BuyRecipeButton(item); });
        }
    }

    private void SetPurchasables()
    {
        foreach (var item in buyables)
        {
            Destroy(item.gameObject);
        }
        buyables.Clear();

        foreach (Item item in buyableItems)
        {
            PurchasableItem newButton = Instantiate(craftingItem, purchasablesMenu.transform);
            buyables.Add(newButton);
            newButton.FillSlot(null, item);
        }

        foreach (PurchasableItem item in buyables)
        {
            item.CraftingItemButton.onClick.AddListener(delegate { BuyItemButton(item); });
        }
    }

    private void SetMenuWhenOpened()
    {
        purchasablesMenu.gameObject.SetActive(true);
        sellMenu.gameObject.SetActive(false);
        swapButtonText.text = "Sell";

        if (setToRecipes)
            SetRecipes();
        else
            SetPurchasables();
    }

    public void SwapMenu()
    {
        purchasablesMenu.gameObject.SetActive(!purchasablesMenu.gameObject.activeInHierarchy);
        sellMenu.gameObject.SetActive(!sellMenu.gameObject.activeInHierarchy);

        if (purchasablesMenu.gameObject.activeInHierarchy)
        {
            if (setToRecipes)
                SetRecipes();
            else
                SetPurchasables();

            swapButtonText.text = "Sell";
        }
        else
        {
            playerInventory.PopulateInventory();
            swapButtonText.text = "Buy";
        }

        if (setToRecipes)
            SetRecipes();
        else
            SetPurchasables();
    }

    public void SwapBuyableItems(bool value)
    {
        setToRecipes = value;

        if (setToRecipes)
            SetRecipes();
        else
            SetPurchasables();
    }

    private void BuyRecipeButton(PurchasableItem item)
    {
        if(!systems.inventoryManager.unlockedRecipes.Contains(item.craftable) &&
            systems.inventoryManager.lockedRecipes.Contains(item.craftable) &&
                systems.statManager.goldAmount >= item.craftable.Two.purchasePrice)
        {
            systems.statManager.goldAmount -= item.craftable.Two.purchasePrice;
            systems.inventoryManager.lockedRecipes.Remove(item.craftable);
            systems.inventoryManager.unlockedRecipes.Add(item.craftable);
            SetRecipes();
            systems.updateCurrencyEvent.Invoke();
        }
    }

    private void BuyItemButton(PurchasableItem item)
    {
        if (systems.inventoryManager.CanAdd(item.item) && systems.statManager.goldAmount >= item.item.purchasePrice)
        {
            systems.inventoryManager.Add(item.item);
            systems.statManager.goldAmount -= item.item.purchasePrice;
            systems.updateCurrencyEvent.Invoke();
        }
    }

    public void SellButton(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty == false)
        {
            float goldToAdd = inventoryItem.itemStack.Two * inventoryItem.itemStack.One.value;
            systems.statManager.goldAmount += goldToAdd;
            systems.updateCurrencyEvent.Invoke();
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
