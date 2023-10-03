using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CraftingBenchCanvas : MonoBehaviour
{
    public List<CraftingItem> recipeButtons = new List<CraftingItem>();

    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;
    [SerializeField] private TMP_Text itemRecipe = null;

    private Systems systems = null;
    private Craftable selectedCraftable = null;


    private void Start()
    {
        systems = Systems.Instance;
    }

    public void SetClickEvents()
    {
        foreach (CraftingItem recipeButton in recipeButtons)
        {
            recipeButton.CraftingItemButton.onClick.AddListener(delegate { SelectRecipeToCraft(recipeButton); });
        }
    }

    public void Craft()
    {
        if (selectedCraftable == null)
            return;

        foreach (ItemStack stack in selectedCraftable.recipe)
        {
            for (int i = 0; i < stack.quantity; i++)
            {
                if (!systems.inventoryManager.CanRemove(stack.item))
                    return;
            }
        }

        systems.inventoryManager.Add(selectedCraftable.result, null);
    }

    private void SelectRecipeToCraft(CraftingItem recipeButton)
    {
        selectedCraftable = recipeButton.craftable;

        itemName.text = selectedCraftable.result.name;
        itemDescription.text = selectedCraftable.result.description;
        itemRecipe.text = "Recipe:\n";

        foreach (ItemStack itemStack in selectedCraftable.recipe)
        {
            itemRecipe.text += $"{itemStack.item.name} - {itemStack.quantity}\n";
        }
    }
}
