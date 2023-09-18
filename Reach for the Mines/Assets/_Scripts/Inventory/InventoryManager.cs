using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    [SerializeField] private int maxStackAmount = 50;
    [SerializeField] private int maxSlots = 32;

    public void Add(Item item, GameObject worldItem)
    {
        if (items.ContainsKey(item) && items[item] < maxStackAmount)
        {
            items[item]++;
            Destroy(worldItem);
        }
        else if (items.Count < maxSlots)
        {
            items.Add(item, 1);
            Destroy(worldItem);
        }
        else
        {
            Debug.Log("inventory full");
        }
    }
}
