using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CraftingItem : MonoBehaviour
{
    public Craftable craftable = null;
    [SerializeField] private Image itemImage = null;
    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;
    [SerializeField] private Button button;

    public void FillSlot(Craftable pItem)
    {
        craftable = pItem;
        itemImage.sprite = pItem.Two.icon;
        itemName.text = pItem.Two.name;

        if(itemDescription!=null)
        itemDescription.text = pItem.Two.description;
    }

    public Button CraftingItemButton => button;
}
