using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingBenchUI : MonoBehaviour
{
    public List<PurchasableItem> recipeButtons = new List<PurchasableItem>();

    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;
    [SerializeField] private TMP_Text itemRecipe = null;

    private CraftingBench craftingBench = null;

    private void Start()
    {
        craftingBench = GetComponent<CraftingBench>();
    }

    public void SetClickEvents()
    {
        foreach (PurchasableItem recipeButton in recipeButtons)
        {
            recipeButton.CraftingItemButton.onClick.AddListener(delegate { SelectRecipeToCraft(recipeButton); });
        }
    }

    private void SelectRecipeToCraft(PurchasableItem recipeButton)
    {
        craftingBench.selectedCraftable = recipeButton.craftable;

        itemName.text = craftingBench.selectedCraftable.Two.name;
        itemDescription.text = craftingBench.selectedCraftable.Two.description;
        itemRecipe.text = "Recipe:\n";

        foreach (ItemStack itemStack in craftingBench.selectedCraftable.One)
        {
            itemRecipe.text += $"{itemStack.One.name} - {itemStack.Two}\n";
        }
    }
}
