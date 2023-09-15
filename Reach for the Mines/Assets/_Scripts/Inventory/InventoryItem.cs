using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item item = null;

    [SerializeField] private Image itemImage = null;
    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;

    public void FillSlot(Item pItem)
    {
        item = pItem;
        itemImage.sprite = item.icon;
        itemImage.gameObject.SetActive(true);
        itemName.text = item.name;
        itemDescription.text = item.description;
    }

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        itemName.text = null;
        itemDescription.text = null;
    }
}
