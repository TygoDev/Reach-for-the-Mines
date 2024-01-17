using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PurchasableItem : MonoBehaviour
{
    public Craftable craftable = null;
    public Item item = null;
    [SerializeField] private Image itemImage = null;
    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;
    [SerializeField] private TMP_Text cost = null;
    [SerializeField] private Button button;

    public void FillSlot(Craftable pCraftable, Item pItem)
    {
        if (pCraftable != null)
            FillRecipe(pCraftable);
        else
            FillItem(pItem);
    }

    private void FillRecipe(Craftable pCraftable)
    {
        craftable = pCraftable;
        itemImage.sprite = pCraftable.Two.icon;
        itemName.text = pCraftable.Two.name;

        if (itemDescription != null)
            itemDescription.text = pCraftable.Two.description;

        if (cost != null)
            cost.text = $"Cost: G:{pCraftable.Two.purchasePrice}";
    }

    private void FillItem(Item pItem)
    {
        item = pItem;
        itemImage.sprite = pItem.icon;
        itemName.text = pItem.name;

        if (itemDescription != null)
            itemDescription.text = pItem.description;

        if (cost != null)
            cost.text = $"Cost: G:{pItem.purchasePrice}";
    }

    public Button CraftingItemButton => button;
}
