using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageCanvas : MonoBehaviour
{
    [SerializeField] private Item stationItem = null;
    [SerializeField] private StationInventory playerInventory = null;
    [SerializeField] private StationInventory storageInventory = null;

    [SerializeField] private Image playerInventoryMenu = null;
    [SerializeField] private Image storageInventoryMenu = null;
    [SerializeField] private TMP_Text swapButtonText = null;

    private Systems systems;

    private void Start()
    {
        systems = Systems.Instance;
        Initialize();
    }

    public void PickUpStation()
    {
        Destroy(transform.parent.gameObject);
        Systems.Instance.inventoryManager.Add(stationItem);
        Systems.Instance.stateManager.UpdateGameState(GameState.Gameplay);
    }

    public void Initialize()
    {
        systems.stateManager.onGameStateChanged += OnGameStateChange;

        playerInventory.itemStacks = systems.inventoryManager.items;

        foreach (InventoryItem item in storageInventory.inventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { TransferSlotContents(item, storageInventory, playerInventory); });
        }

        foreach (InventoryItem item in playerInventory.inventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { TransferSlotContents(item, playerInventory, storageInventory); });
        }
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    private void SetMenuWhenOpened()
    {
        storageInventory.PopulateInventory();
        playerInventory.PopulateInventory();
        storageInventoryMenu.gameObject.SetActive(true);
        playerInventoryMenu.gameObject.SetActive(false);
        swapButtonText.text = "Inventory";
    }

    public void SwapMenu()
    {
        playerInventoryMenu.gameObject.SetActive(!playerInventoryMenu.gameObject.activeInHierarchy);
        storageInventoryMenu.gameObject.SetActive(!storageInventoryMenu.gameObject.activeInHierarchy);

        if (storageInventoryMenu.gameObject.activeInHierarchy)
        {
            storageInventory.PopulateInventory();
            swapButtonText.text = "Inventory";
        }
        else
        {
            playerInventory.PopulateInventory();
            swapButtonText.text = "Storage";
        }
    }

    public void TransferSlotContents(InventoryItem pInvItem, StationInventory inventoryFrom, StationInventory inventoryTo)
    {
        if (!pInvItem.empty)
        {
            ItemStack tempItemStack = new ItemStack(pInvItem.itemStack.One, pInvItem.itemStack.Two);
            ItemStack existingItemStack = inventoryFrom.itemStacks.Find(i => i.One == pInvItem.item);

            for (int i = 0; i < pInvItem.itemStack.Two; i++)
            {
                if(inventoryTo.CanAdd(pInvItem.itemStack.One))
                {
                    inventoryTo.Add(pInvItem.itemStack.One);
                    tempItemStack.Two--;
                }
            }

            existingItemStack.Two = tempItemStack.Two;

            pInvItem.ClearSlot();

            if (tempItemStack.Two > 0)
                pInvItem.FillSlot(tempItemStack);
            else
                inventoryFrom.RemoveStack(existingItemStack);

            storageInventory.PopulateInventory();
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
