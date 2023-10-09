using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StorageCanvas : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField] private Image playerInventory = null;
    [SerializeField] private Image storageInventory = null;

    private Systems systems;

    private void Start()
    {
        systems = Systems.Instance;
        PopulateInventory();
    }

    public void SwapMenu()
    {
        PopulateInventory();
        playerInventory.gameObject.SetActive(!playerInventory.gameObject.activeInHierarchy);
        storageInventory.gameObject.SetActive(!storageInventory.gameObject.activeInHierarchy);
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
}
