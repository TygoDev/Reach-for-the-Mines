using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    [SerializeField] private int slotLimit = 32;

    public void Add(Item item, GameObject worldItem)
    {
        if (items.Count < slotLimit)
        {
            Destroy(worldItem);
            items.Add(item);
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
