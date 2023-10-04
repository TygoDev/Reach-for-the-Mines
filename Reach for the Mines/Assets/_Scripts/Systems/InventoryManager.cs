using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;

    public List<Craftable> unlockedRecipes = new List<Craftable>();

    public void Add(Item item, GameObject worldItem = null)
    {
        if(!CanAdd(item))
            return;

        ItemStack existingItem = items.Find(i => i.item == item && i.quantity < maxStackAmount);

        if (existingItem != null && existingItem.quantity < maxStackAmount)
        {
            existingItem.quantity++;

            if (worldItem != null)
                Destroy(worldItem);
        }
        else if (items.Count < maxSlots)
        {
            items.Add(new ItemStack(item, 1));

            if (worldItem != null)
                Destroy(worldItem);
        }
    }

    public bool CanAdd(Item item)
    {
        ItemStack existingItem = items.Find(i => i.item == item && i.quantity < maxStackAmount);

        if (existingItem != null && existingItem.quantity < maxStackAmount)
        {
            return true;
        }
        else if (items.Count < maxSlots)
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
        ItemStack existingItem = items.Find(i => i.item == item);

        if (existingItem != null && existingItem.quantity > 0)
        {
            existingItem.quantity--;
        }

        if(existingItem.quantity == 0)
            RemoveStack(existingItem);
    }

    public bool CanRemove(Item item)
    {
        ItemStack existingItem = items.Find(i => i.item == item);

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
        if (items.Contains(itemStack))
        {
            items.Remove(itemStack);
        }
    }
}

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int quantity;

    public ItemStack(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

[System.Serializable]
public class Craftable
{
    public List<ItemStack> recipe;
    public Item result;

    public Craftable(List<ItemStack> recipe, Item result)
    {
        this.recipe = recipe;
        this.result = result;
    }
}