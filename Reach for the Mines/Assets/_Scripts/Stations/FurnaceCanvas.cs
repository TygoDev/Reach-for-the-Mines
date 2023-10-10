using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FurnaceCanvas : MonoBehaviour
{
    [SerializeField] private List<Smeltable> smeltables = new List<Smeltable>();
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    [SerializeField] private InventoryItem fuelSlot = null;
    [SerializeField] private InventoryItem inputSlot = null;
    [SerializeField] private InventoryItem outputSlot = null;
    [SerializeField] private Image furnaceMenu = null;
    [SerializeField] private Image inventoryMenu = null;

    private Systems systems = default;
    private InventoryItem slotToFill = null;
    private bool isProcessing = false;

    private void Start()
    {
        systems = Systems.Instance;
        systems.stateManager.onGameStateChanged += OnGameStateChange;
    }

    private void OnDisable()
    {
        systems.stateManager.onGameStateChanged -= OnGameStateChange;
    }

    public void SwapMenu()
    {
        PopulateInventory();
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
        if (!inventoryItem.empty && slotToFill.empty)
        {
            if (inventoryItem.item.itemType == ItemType.Smeltable && slotToFill == inputSlot)
            {
                SwapMenu();
                inputSlot.FillSlot(inventoryItem.itemStack);
                systems.inventoryManager.RemoveStack(inventoryItem.itemStack);
                PopulateInventory();
                slotToFill = null;
                StartCoroutine(Smelt());
            }
            else if (inventoryItem.item.itemType == ItemType.Fuel && slotToFill == fuelSlot)
            {
                SwapMenu();
                fuelSlot.FillSlot(inventoryItem.itemStack);
                systems.inventoryManager.RemoveStack(inventoryItem.itemStack);
                PopulateInventory();
                slotToFill = null;
                StartCoroutine(Smelt());
            }
        }
    }

    public void Process()
    {
        if (!isProcessing)
            return;

        foreach (Smeltable smeltable in smeltables)
        {
            if (smeltable.itemInput == inputSlot.item)
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
            outputSlot.FillSlot(new ItemStack(smeltable.itemOutput, 1));
            return true;
        }
        else if (outputSlot.item == smeltable.itemOutput)
        {
            ItemStack newStackOutput = outputSlot.itemStack;
            newStackOutput.quantity++;
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
        newStackInput.quantity -= 5;
        inputSlot.ClearSlot();

        if (newStackInput.quantity != 0)
            inputSlot.FillSlot(newStackInput);
    }

    private void HandleFuelSlot()
    {
        ItemStack newStackFuel = fuelSlot.itemStack;
        newStackFuel.quantity--;
        fuelSlot.ClearSlot();

        if (newStackFuel.quantity != 0)
            fuelSlot.FillSlot(newStackFuel);
    }

    public void TakeSlotContents(InventoryItem inventoryItem)
    {
        if (!inventoryItem.empty)
        {
            ItemStack itemStack = new ItemStack(inventoryItem.itemStack.item,inventoryItem.itemStack.quantity);

            for (int i = 0; i < inventoryItem.itemStack.quantity; i++)
            {
                if (systems.inventoryManager.CanAdd(inventoryItem.itemStack.item))
                {
                    systems.inventoryManager.Add(inventoryItem.itemStack.item, null);
                    itemStack.quantity--;
                }
            }

            inventoryItem.ClearSlot();

            if (itemStack.quantity > 0)
                inventoryItem.FillSlot(itemStack);

            PopulateInventory();
        }
    }

    private void PopulateInventory()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItems[i].ClearSlot();
            if (systems.inventoryManager.items.Count > i)
                inventoryItems[i].FillSlot(systems.inventoryManager.items.ElementAt(i));
        }
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
        inputSlot.itemStack.quantity >= 5 && fuelSlot.itemStack.quantity >= 1 &&
        !isProcessing;
    }


    // EVENT LISTENERS
    public void OnGameStateChange(GameState state)
    {
        if (state == GameState.Menu && GetComponent<Canvas>().enabled == true)
        {
            furnaceMenu.gameObject.SetActive(true);
            inventoryMenu.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class Smeltable
{
    public Item itemInput;
    public Item itemOutput;
    public Smeltable(Item pitemInput, Item pitemOutput)
    {
        this.itemInput = pitemInput;
        this.itemOutput = pitemOutput;
    }
}

