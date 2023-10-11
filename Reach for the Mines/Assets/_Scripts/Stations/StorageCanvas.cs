using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageCanvas : MonoBehaviour
{
    public List<ItemStack> chestItems = new List<ItemStack>();

    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField] private List<InventoryItem> chestInventoryItems = new List<InventoryItem>();

    [SerializeField] private Image playerInventory = null;
    [SerializeField] private Image storageInventory = null;
    [SerializeField] private TMP_Text swapButtonText = null;

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;

    private Systems systems;

    private void Start()
    {
        systems = Systems.Instance;
        Subscribe();

        PopulateInventory(inventoryItems, systems.inventoryManager.items);
        PopulateInventory(chestInventoryItems, chestItems);

        foreach (InventoryItem item in chestInventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { TransferSlotContents(item, systems.inventoryManager.items, chestItems); });
        }

        foreach (InventoryItem item in inventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { TransferSlotContents(item, chestItems, systems.inventoryManager.items); });
        }
    }

    private void Subscribe()
    {
        systems.stateManager.onGameStateChanged += OnGameStateChange;
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    public void SwapMenu()
    {
        playerInventory.gameObject.SetActive(!playerInventory.gameObject.activeInHierarchy);
        storageInventory.gameObject.SetActive(!storageInventory.gameObject.activeInHierarchy);

        if (storageInventory.gameObject.activeInHierarchy)
        {
            PopulateInventory(chestInventoryItems, chestItems);
            swapButtonText.text = "Inventory";
        }
        else
        {
            PopulateInventory(inventoryItems, systems.inventoryManager.items);
            swapButtonText.text = "Storage";
        }
    }

    private void PopulateInventory(List<InventoryItem> pInvItems, List<ItemStack> pItems)
    {
        for (int i = 0; i < pInvItems.Count; i++)
        {
            pInvItems[i].ClearSlot();
            if (pItems.Count > i)
                pInvItems[i].FillSlot(pItems.ElementAt(i));
        }
    }

    public void TransferSlotContents(InventoryItem pInvItem, List<ItemStack> pTargetInv, List<ItemStack> pInvToTakeFrom)
    {
        if (!pInvItem.empty)
        {
            ItemStack tempItemStack = new ItemStack(pInvItem.itemStack.One, pInvItem.itemStack.Two);
            ItemStack existingItemStack = pInvToTakeFrom.Find(i => i.One == pInvItem.item);

            for (int i = 0; i < pInvItem.itemStack.Two; i++)
            {
                if(CanAdd(pInvItem.itemStack.One, pTargetInv))
                {
                    Add(pInvItem.itemStack.One, pTargetInv);
                    tempItemStack.Two--;
                }
            }

            existingItemStack.Two = tempItemStack.Two;

            pInvItem.ClearSlot();

            if (tempItemStack.Two > 0)
                pInvItem.FillSlot(tempItemStack);
            else
                pInvToTakeFrom.Remove(existingItemStack);

            PopulateInventory(chestInventoryItems, chestItems);
            PopulateInventory(inventoryItems, systems.inventoryManager.items);
        }
    }

    public void Add(Item item, List<ItemStack> targetInventory)
    {
        if (!CanAdd(item, targetInventory))
            return;

        ItemStack existingItem = targetInventory.Find(i => i.One == item && i.Two < maxStackAmount);

        if (existingItem != null && existingItem.Two < maxStackAmount)
        {
            existingItem.Two++;
        }
        else if (targetInventory.Count < maxSlots)
        {
            targetInventory.Add(new ItemStack(item, 1));
        }
    }

    public bool CanAdd(Item item, List<ItemStack> targetItemList)
    {
        ItemStack existingItem = targetItemList.Find(i => i.One == item && i.Two < maxStackAmount);

        if (existingItem != null && existingItem.Two < maxStackAmount)
        {
            return true;
        }
        else if (targetItemList.Count < maxSlots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(Item item)
    {
        ItemStack existingItem = chestItems.Find(i => i.One == item);

        if (existingItem != null && existingItem.Two > 0)
        {
            existingItem.Two--;
        }

        if (existingItem.Two == 0)
            RemoveStack(existingItem);
    }

    public bool CanRemove(Item item)
    {
        ItemStack existingItem = chestItems.Find(i => i.One == item);

        if (existingItem != null && existingItem.Two > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveStack(ItemStack itemStack)
    {
        if (chestItems.Contains(itemStack))
        {
            chestItems.Remove(itemStack);
        }
    }

    //LISTENERS

    public void OnGameStateChange(GameState state)
    {
        if (state == GameState.Menu && GetComponent<Canvas>().enabled == true)
        {
            PopulateInventory(chestInventoryItems, chestItems);
            PopulateInventory(inventoryItems, systems.inventoryManager.items);
            storageInventory.gameObject.SetActive(true);
            playerInventory.gameObject.SetActive(false);
            swapButtonText.text = "Inventory";
        }
    }
}
