using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Anvil))]
public class AnvilUI : MonoBehaviour
{
    public List<PurchasableItem> recipeButtons = new List<PurchasableItem>();

    [SerializeField] private TMP_Text itemName = null;
    [SerializeField] private TMP_Text itemDescription = null;
    [SerializeField] private TMP_Text itemRecipe = null;

    private Anvil anvil = null;

    private void Start()
    {
        anvil = GetComponent<Anvil>();
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
        anvil.selectedCraftable = recipeButton.craftable;

        itemName.text = anvil.selectedCraftable.Two.name;
        itemDescription.text = anvil.selectedCraftable.Two.description;
        itemRecipe.text = "Recipe:\n";

        foreach (ItemStack itemStack in anvil.selectedCraftable.One)
        {
            itemRecipe.text += $"{itemStack.One.name} - {itemStack.Two}\n";
        }
    }
}
