using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPickaxeRecipes : MonoBehaviour
{
    [SerializeField] private AnvilUI anvilUI = null;
    [SerializeField] private PurchasableItem craftingItem = null;

    private Systems systems = null;

    private void Start()
    {
        Initialize();

    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        anvilUI = GetComponentInParent<AnvilUI>();
        systems = Systems.Instance;
        InitializeRecipes();
        Subscribe();
    }

    private void Subscribe()
    {
        systems.stateManager.onGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChanged;
    }

    private void OnDestroy()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChanged;
    }

    private void InitializeRecipes()
    {
        foreach (Craftable craftable in systems.inventoryManager.unlockedRecipes)
        {
            if (craftable.Two.itemType == ItemType.Pickaxe)
            {
                PurchasableItem newButton = Instantiate(craftingItem, transform);
                newButton.FillSlot(craftable, null);
                anvilUI.recipeButtons.Add(newButton);
            }
        }

        anvilUI.SetClickEvents();
    }

    // -------------- EVENT LISTENERS -------------- 

    private void OnGameStateChanged(GameState state)
    {
        if (anvilUI == null)
            anvilUI = GetComponentInParent<AnvilUI>();

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
