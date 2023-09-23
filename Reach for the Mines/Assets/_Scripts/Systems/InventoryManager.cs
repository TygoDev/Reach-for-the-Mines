using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;

    public void Add(Item item, GameObject worldItem)
    {
        ItemStack existingItem = items.Find(i => i.item == item && i.quantity < maxStackAmount);

        if (existingItem != null && existingItem.quantity < maxStackAmount)
        {
            existingItem.quantity++;
            Destroy(worldItem);
        }
        else if (items.Count < maxSlots)
        {
            items.Add(new ItemStack(item, 1));
            Destroy(worldItem);
        }
        else
        {
            Debug.Log("Inventory full");
        }
    }

    public void Remove(ItemStack itemStack)
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
