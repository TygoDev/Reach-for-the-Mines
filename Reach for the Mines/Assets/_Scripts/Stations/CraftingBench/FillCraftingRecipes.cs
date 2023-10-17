using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillCraftingRecipes : MonoBehaviour
{
    [SerializeField] private CraftingBenchUI craftingBenchUI = null;
    [SerializeField] private CraftingItem craftingItem = null;

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;
        systems.stateManager.onGameStateChanged += OnGameStateChanged;
        InitializeRecipes();
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChanged;
    }

    private void InitializeRecipes()
    {
        foreach (Craftable craftable in systems.inventoryManager.unlockedRecipes)
        {
            CraftingItem newButton = Instantiate(craftingItem, transform);
            newButton.FillSlot(craftable);
            craftingBenchUI.recipeButtons.Add(newButton);
        }

        craftingBenchUI.SetClickEvents();
    }

    // -------------- EVENT LISTENERS -------------- 

    private void OnGameStateChanged(GameState state)
    {
        if(state == GameState.Menu && craftingBenchUI.GetComponent<Canvas>().enabled)
        {
            foreach (var item in craftingBenchUI.recipeButtons)
            {
                Destroy(item.gameObject);
            }
            craftingBenchUI.recipeButtons.Clear();
            InitializeRecipes();
        }
    }
}
