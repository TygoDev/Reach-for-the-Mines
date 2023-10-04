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

        List<ItemStack> tempList = new List<ItemStack>();

        foreach (ItemStack stack in selectedCraftable.recipe)
        {
            ItemStack tempStack = new ItemStack(stack.item,0);

            for (int i = 0; i < stack.quantity; i++)
            {
                if (systems.inventoryManager.CanRemove(stack.item))
                {
                    systems.inventoryManager.Remove(stack.item);
                    tempStack.quantity++;
                    Debug.Log(tempStack.quantity);
                }
                else
                {
                    if(tempList.Count > 0)
                    {
                        foreach (var item in tempList)
                        {
                            for (int k = 0; k < item.quantity; k++)
                            {
                                systems.inventoryManager.Add(item.item);
                            }
                        }
                    }

                    for (int j = 0; j < tempStack.quantity; j++)
                    {
                        systems.inventoryManager.Add(tempStack.item);
                    }
                    return;
                }           
            }
            tempList.Add(tempStack);
        }
        Debug.Log("Crafting");
        systems.inventoryManager.Add(selectedCraftable.result);
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
