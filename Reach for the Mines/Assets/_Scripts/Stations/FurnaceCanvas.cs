using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Utils;
using System;

public class FurnaceCanvas : MonoBehaviour
{
    [SerializeField] private List<Smeltable> smeltables = new List<Smeltable>();
    [SerializeField] private StationInventory playerInventory = null;

    [SerializeField] private InventoryItem fuelSlot = null;
    [SerializeField] private InventoryItem inputSlot = null;
    [SerializeField] private InventoryItem outputSlot = null;

    [SerializeField] private int minInputSlotAmount = 5;
    [SerializeField] private int minFuelSlotAmount = 1;

    [SerializeField] private Image furnaceMenu = null;
    [SerializeField] private Image inventoryMenu = null;

    private Systems systems = default;
    private InventoryItem slotToFill = null;
    private bool isProcessing = false;

    private void Start()
    {
        //Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        systems = Systems.Instance;
        systems.stateManager.onGameStateChanged += OnGameStateChange;

        playerInventory.itemStacks = systems.inventoryManager.items;

        foreach (InventoryItem item in playerInventory.inventoryItems)
        {
            item.InventoryItemButton.onClick.AddListener(delegate { FillSlot(item); });
        }
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    private void OnDestroy()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    public void SwapMenu()
    {
        playerInventory.PopulateInventory();
        furnaceMenu.gameObject.SetActive(!furnaceMenu.gameObject.activeInHierarchy);
        inventoryMenu.gameObject.SetActive(!inventoryMenu.gameObject.activeInHierarchy);
    }

    public void FurnaceSlot(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty)
        {
            slotToFill = inventoryItem;
            SwapMenu();
        }
        else
        {
            isProcessing = false;
            TakeSlotContents(inventoryItem);
        }

    }

    public void FillSlot(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty)
            return;

        if (inventoryItem.empty && !slotToFill.empty)
            return;

        if (inventoryItem.item.itemType == ItemType.Smeltable && slotToFill == inputSlot)
        {
            SwapMenu();
            inputSlot.FillSlot(inventoryItem.itemStack);
            systems.inventoryManager.RemoveStack(inventoryItem.itemStack);
            playerInventory.PopulateInventory();
            slotToFill = null;
            StartCoroutine(Smelt());
        }
        else if (inventoryItem.item.itemType == ItemType.Fuel && slotToFill == fuelSlot)
        {
            SwapMenu();
            fuelSlot.FillSlot(inventoryItem.itemStack);
            systems.inventoryManager.RemoveStack(inventoryItem.itemStack);
            playerInventory.PopulateInventory();
            slotToFill = null;
            StartCoroutine(Smelt());
        }
    }

    public void Process()
    {
        if (!isProcessing)
            return;

        foreach (Smeltable smeltable in smeltables)
        {
            if (smeltable.One == inputSlot.item)
            {
                if (!HandleOutputSlot(smeltable))
                    return;

                HandleInputSlot();
                HandleFuelSlot();

            }
        }
    }

    private bool HandleOutputSlot(Smeltable smeltable)
    {
        if (outputSlot.empty)
        {
            outputSlot.FillSlot(new ItemStack(smeltable.Two, 1));
            return true;
        }
        else if (outputSlot.item == smeltable.Two)
        {
            ItemStack newStackOutput = outputSlot.itemStack;
            newStackOutput.Two++;
            outputSlot.ClearSlot();
            outputSlot.FillSlot(newStackOutput);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void HandleInputSlot()
    {
        ItemStack newStackInput = inputSlot.itemStack;
        newStackInput.Two -= 5;
        inputSlot.ClearSlot();

        if (newStackInput.Two != 0)
            inputSlot.FillSlot(newStackInput);
    }

    private void HandleFuelSlot()
    {
        ItemStack newStackFuel = fuelSlot.itemStack;
        newStackFuel.Two--;
        fuelSlot.ClearSlot();

        if (newStackFuel.Two != 0)
            fuelSlot.FillSlot(newStackFuel);
    }

    public void TakeSlotContents(InventoryItem inventoryItem)
    {
        if (inventoryItem.empty)
            return;

            ItemStack itemStack = new ItemStack(inventoryItem.itemStack.One, inventoryItem.itemStack.Two);

            for (int i = 0; i < inventoryItem.itemStack.Two; i++)
            {
                if (systems.inventoryManager.CanAdd(inventoryItem.itemStack.One))
                {
                    systems.inventoryManager.Add(inventoryItem.itemStack.One, null);
                    itemStack.Two--;
                }
            }

            inventoryItem.ClearSlot();

            if (itemStack.Two > 0)
                inventoryItem.FillSlot(itemStack);

        playerInventory.PopulateInventory();
    }

    private IEnumerator Smelt()
    {
        if (CanSmelt())
        {
            isProcessing = true;
            yield return new WaitForSeconds(5f);
            Process();
            isProcessing = false;
            StartCoroutine(Smelt());
        }
    }

    private bool CanSmelt()
    {
        return !inputSlot.empty && !fuelSlot.empty &&
        inputSlot.itemStack.Two >= minInputSlotAmount && fuelSlot.itemStack.Two >= minFuelSlotAmount &&
        !isProcessing;
    }


    // -------------- EVENT LISTENERS -------------- 
    public void OnGameStateChange(GameState state)
    {
            furnaceMenu.gameObject.SetActive(true);
            inventoryMenu.gameObject.SetActive(false);
    }
}

