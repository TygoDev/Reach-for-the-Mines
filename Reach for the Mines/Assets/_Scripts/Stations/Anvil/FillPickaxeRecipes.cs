using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPickaxeRecipes : MonoBehaviour
{
    [SerializeField] private AnvilUI anvilUI = null;
    [SerializeField] private PurchasableItem craftingItem = null;
    private List<PurchasableItem> existingRecipes = new List<PurchasableItem>();

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;
    }

    private void OnEnable()
    {
        EventBus<StationInteractedEvent>.OnEvent += InitializeRecipes;
    }

    private void OnDisable()
    {
        EventBus<StationInteractedEvent>.OnEvent -= InitializeRecipes;
    }

    private void InitializeRecipes(StationInteractedEvent stationInteractedEvent)
    {
        if (stationInteractedEvent.station.name == "Anvil(Clone)")
        {
            foreach (Craftable craftable in systems.inventoryManager.unlockedRecipes)
            {
                foreach (var item in existingRecipes)
                {
                    if (item.craftable == craftable)
                    {
                        goto GoNext;
                    }
                }

                if (craftable.Two.itemType == ItemType.Pickaxe)
                {
                    PurchasableItem newButton = Instantiate(craftingItem, transform);
                    newButton.FillSlot(craftable, null);
                    anvilUI.recipeButtons.Add(newButton);
                    existingRecipes.Add(newButton);
                }

            GoNext:;

            }

            anvilUI.SetClickEvents();
        }
    }
}
