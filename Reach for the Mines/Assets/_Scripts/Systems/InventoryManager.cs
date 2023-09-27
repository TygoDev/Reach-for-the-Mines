using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();
    public bool inventoryIsFull = false;

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;

    public void Add(Item item, GameObject worldItem)
    {
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
        else
        {
            inventoryIsFull = true;
            Debug.Log("Inventory full");
        }
    }

    public void Remove(ItemStack itemStack)
    {
        if (items.Contains(itemStack))
        {
            inventoryIsFull = false;
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
