using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CraftingBench))]
public class CraftingBenchUI : MonoBehaviour
{
    public List<CraftingItem> recipeButtons = new List<CraftingItem>();

    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;
    [SerializeField] private TMP_Text itemRecipe = null;

    private CraftingBench craftingBenchCanvas = null;

    private void Start()
    {
        craftingBenchCanvas = GetComponent<CraftingBench>();
    }

    public void SetClickEvents()
    {
        foreach (CraftingItem recipeButton in recipeButtons)
        {
            recipeButton.CraftingItemButton.onClick.AddListener(delegate { SelectRecipeToCraft(recipeButton); });
        }
    }

    private void SelectRecipeToCraft(CraftingItem recipeButton)
    {
        craftingBenchCanvas.selectedCraftable = recipeButton.craftable;

        itemName.text = craftingBenchCanvas.selectedCraftable.result.name;
        itemDescription.text = craftingBenchCanvas.selectedCraftable.result.description;
        itemRecipe.text = "Recipe:\n";

        foreach (ItemStack itemStack in craftingBenchCanvas.selectedCraftable.recipe)
        {
            itemRecipe.text += $"{itemStack.item.name} - {itemStack.quantity}\n";
        }
    }
}
