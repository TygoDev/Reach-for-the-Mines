using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationInventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<ItemStack> itemStacks = new List<ItemStack>();

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;

    Systems systems = null;

    private void Awake()
    {
        systems = Systems.Instance;
        PopulateInventory();
    }

    public void PopulateInventory()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].ClearSlot();
            if (itemStacks.Count > i)
                inventoryItems[i].FillSlot(itemStacks.ElementAt(i));
        }
    }

    public void Add(Item item)
    {
        if (!CanAdd(item))
            return;

        ItemStack existingItem = itemStacks.Find(i => i.One == item && i.Two < maxStackAmount);

        if (existingItem != null && existingItem.Two < maxStackAmount)
        {
            existingItem.Two++;
        }
        else if (itemStacks.Count < maxSlots)
        {
            itemStacks.Add(new ItemStack(item, 1));
        }
    }

    public bool CanAdd(Item item)
    {
        ItemStack existingItem = itemStacks.Find(i => i.One == item && i.Two < maxStackAmount);

        if (existingItem != null && existingItem.Two < maxStackAmount)
        {
            return true;
        }
        else if (itemStacks.Count < maxSlots)
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
        if (!CanRemove(item))
            return;

        ItemStack existingItem = itemStacks.Find(i => i.One == item);

        if (existingItem != null && existingItem.Two > 0)
        {
            existingItem.Two--;
        }

        if (existingItem.Two == 0)
            RemoveStack(existingItem);
    }

    public bool CanRemove(Item item)
    {
        ItemStack existingItem = itemStacks.Find(i => i.One == item);

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
        if (itemStacks.Contains(itemStack))
        {
            itemStacks.Remove(itemStack);
        }
    }
}
