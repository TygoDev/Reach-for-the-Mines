using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();
    public List<Craftable> unlockedRecipes = new List<Craftable>();
    public event UnityAction itemPickedUpEvent = delegate { };

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;



    public void Add(Item item, GameObject worldItem = null)
    {
        if(!CanAdd(item))
            return;

        ItemStack existingItem = items.Find(i => i.One == item && i.Two < maxStackAmount);

        if (existingItem != null && existingItem.Two < maxStackAmount)
        {
            existingItem.Two++;

            if (worldItem != null)
                Destroy(worldItem);
        }
        else if (items.Count < maxSlots)
        {
            items.Add(new ItemStack(item, 1));

            if (worldItem != null)
                Destroy(worldItem);
        }

        itemPickedUpEvent.Invoke();
    }

    public bool CanAdd(Item item)
    {
        ItemStack existingItem = items.Find(i => i.One == item && i.Two < maxStackAmount);

        if (existingItem != null && existingItem.Two < maxStackAmount)
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
        ItemStack existingItem = items.Find(i => i.One == item);

        if (existingItem != null && existingItem.Two > 0)
        {
            existingItem.Two--;
        }

        if(existingItem.Two == 0)
            RemoveStack(existingItem);
    }

    public bool CanRemove(Item item)
    {
        ItemStack existingItem = items.Find(i => i.One == item);

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
        if (items.Contains(itemStack))
        {
            items.Remove(itemStack);
        }
    }
}