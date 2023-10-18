using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPickaxeRecipes : MonoBehaviour
{
    [SerializeField] private AnvilUI anvilUI = null;
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
            if(craftable.Two.itemType == ItemType.Pickaxe)
            {
                CraftingItem newButton = Instantiate(craftingItem, transform);
                newButton.FillSlot(craftable);
                anvilUI.recipeButtons.Add(newButton);
            }
        }

        anvilUI.SetClickEvents();
    }

    // -------------- EVENT LISTENERS -------------- 

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Menu && anvilUI.GetComponent<Canvas>().enabled)
        {
            foreach (var item in anvilUI.recipeButtons)
            {
                Destroy(item.gameObject);
            }
            anvilUI.recipeButtons.Clear();
            InitializeRecipes();
        }
    }
}
