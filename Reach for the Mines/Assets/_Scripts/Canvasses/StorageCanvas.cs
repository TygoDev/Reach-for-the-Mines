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

        PopulateInventory();
        PopulateChest();

        foreach (InventoryItem item in chestInventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { TakeSlotContents(item); });
        }

        foreach (InventoryItem item in inventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { InsertSlotContents(item); });
        }
    }

    public void SwapMenu()
    {
        playerInventory.gameObject.SetActive(!playerInventory.gameObject.activeInHierarchy);
        storageInventory.gameObject.SetActive(!storageInventory.gameObject.activeInHierarchy);

        if (storageInventory.gameObject.activeInHierarchy)
        {
            PopulateChest();
            swapButtonText.text = "Inventory";
        }
        else
        {
            PopulateInventory();
            swapButtonText.text = "Storage";
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

    private void PopulateChest()
    {
        for (int i = 0; i < chestInventoryItems.Count; i++)
        {
            chestInventoryItems[i].ClearSlot();
            if (chestItems.Count > i)
                chestInventoryItems[i].FillSlot(chestItems[i]);
        }
    }

    public void TakeSlotContents(InventoryItem inventoryItem)
    {
        if (!inventoryItem.empty)
        {
            ItemStack itemStack = new ItemStack(inventoryItem.itemStack.item, inventoryItem.itemStack.quantity);
            ItemStack existingItem = chestItems.Find(i => i.item == inventoryItem.itemStack.item);

            for (int i = 0; i < inventoryItem.itemStack.quantity; i++)
            {
                if (systems.inventoryManager.CanAdd(inventoryItem.itemStack.item))
                {
                    systems.inventoryManager.Add(inventoryItem.itemStack.item, null);
                    itemStack.quantity--;
                }
            }

            existingItem.quantity = itemStack.quantity;

            inventoryItem.ClearSlot();

            if (itemStack.quantity > 0)
                inventoryItem.FillSlot(itemStack);
            else
                chestItems.Remove(existingItem);

            PopulateChest();
            PopulateInventory();
        }
    }

    public void InsertSlotContents(InventoryItem inventoryItem)
    {
        if (!inventoryItem.empty)
        {
            ItemStack itemStack = new ItemStack(inventoryItem.itemStack.item, inventoryItem.itemStack.quantity);
            ItemStack existingItem = systems.inventoryManager.items.Find(i => i.item == inventoryItem.itemStack.item);

            for (int i = 0; i < inventoryItem.itemStack.quantity; i++)
            {
                if (CanAdd(inventoryItem.itemStack.item))
                {
                    Add(inventoryItem.itemStack.item, null);
                    itemStack.quantity--;
                }
            }

            existingItem.quantity = itemStack.quantity;

            inventoryItem.ClearSlot();

            if (itemStack.quantity > 0)
                inventoryItem.FillSlot(itemStack);
            else
                systems.inventoryManager.items.Remove(existingItem);

            PopulateChest();
            PopulateInventory();
        }
    }

    public void Add(Item item, GameObject worldItem = null)
    {
        if (!CanAdd(item))
            return;

        ItemStack existingItem = chestItems.Find(i => i.item == item && i.quantity < maxStackAmount);

        if (existingItem != null && existingItem.quantity < maxStackAmount)
        {
            existingItem.quantity++;

            if (worldItem != null)
                Destroy(worldItem);
        }
        else if (chestItems.Count < maxSlots)
        {
            chestItems.Add(new ItemStack(item, 1));

            if (worldItem != null)
                Destroy(worldItem);
        }
    }

    public bool CanAdd(Item item)
    {
        ItemStack existingItem = chestItems.Find(i => i.item == item && i.quantity < maxStackAmount);

        if (existingItem != null && existingItem.quantity < maxStackAmount)
        {
            return true;
        }
        else if (chestItems.Count < maxSlots)
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
        ItemStack existingItem = chestItems.Find(i => i.item == item);

        if (existingItem != null && existingItem.quantity > 0)
        {
            existingItem.quantity--;
        }

        if (existingItem.quantity == 0)
            RemoveStack(existingItem);
    }

    public bool CanRemove(Item item)
    {
        ItemStack existingItem = chestItems.Find(i => i.item == item);

        if (existingItem != null && existingItem.quantity > 0)
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
}
