using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillCraftingRecipes : MonoBehaviour
{
    [SerializeField] private CraftingBenchCanvas craftingBenchCanvas = null;
    [SerializeField] private CraftingItem craftingItem = null;

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

        foreach (Craftable craftable in systems.inventoryManager.unlockedRecipes)
        {
            CraftingItem newButton = Instantiate(craftingItem, transform);
            newButton.FillSlot(craftable);
            craftingBenchCanvas.recipeButtons.Add(newButton);
        }

        craftingBenchCanvas.SetClickEvents();
    }


}
