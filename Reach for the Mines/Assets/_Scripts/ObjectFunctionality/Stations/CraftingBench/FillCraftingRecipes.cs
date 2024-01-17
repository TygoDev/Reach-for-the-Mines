using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillCraftingRecipes : MonoBehaviour
{
    [SerializeField] private CraftingBenchUI craftingBenchUI = null;
    [SerializeField] private PurchasableItem craftingItem = null;
    private List<PurchasableItem> existingRecipes = new List<PurchasableItem>();

    private Systems systems = null;

    private void OnEnable()
    {
        systems = Systems.Instance;

        EventBus<StationInteractedEvent>.OnEvent += InitializeRecipes;
    }

    private void OnDisable()
    {
        EventBus<StationInteractedEvent>.OnEvent -= InitializeRecipes;
    }

    private void InitializeRecipes(StationInteractedEvent stationInteractedEvent)
    {
        if (stationInteractedEvent.station.name == "Crafting Bench(Clone)")
        {
            foreach (Craftable craftable in systems.inventoryManager.unlockedRecipes)
            {
                if (craftable.Two.itemType != ItemType.Pickaxe)
                {
                    foreach (var item in existingRecipes)
                    {
                        if(item.craftable == craftable)
                        {
                            goto GoNext;
                        }
                    }

                    PurchasableItem newButton = Instantiate(craftingItem, transform);
                    newButton.FillSlot(craftable, null);
                    craftingBenchUI.recipeButtons.Add(newButton);
                    existingRecipes.Add(newButton);

                GoNext:;
                   
                }
            }

            craftingBenchUI.SetClickEvents();
        }
    }
}
