using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item item = null;
    public ItemStack itemStack = null;
    public bool empty = true;

    [SerializeField] private Image itemImage = null;
    [SerializeField] private TMP_Text itemAmount = null;
    
    public void FillSlot(ItemStack pItem)
    {
        empty = false;

        item = pItem.item;
        itemImage.sprite = item.icon;
        itemImage.gameObject.SetActive(true);
        itemAmount.text = pItem.quantity.ToString();
        itemStack = pItem;
        itemAmount.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        empty = true;

        item = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        itemAmount.text = null;
        itemStack = null;
        itemAmount.gameObject.SetActive(false);
    }
}
