using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

[RequireComponent(typeof(AnvilUI))]
public class Anvil : MonoBehaviour
{
    public Craftable selectedCraftable = null;
    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;
    }

    public void Craft()
    {
        if (selectedCraftable.Two == null)
            return;

        if (systems.inventoryManager.currentPickaxe.id + 1 != selectedCraftable.Two.id)
            return;

        List<ItemStack> tempList = new List<ItemStack>();

        foreach (ItemStack stack in selectedCraftable.One)
        {
            ItemStack tempStack = new ItemStack(stack.One,0);

            for (int i = 0; i < stack.Two; i++)
            {
                if (systems.inventoryManager.CanRemove(stack.One))
                {
                    systems.inventoryManager.Remove(stack.One);
                    tempStack.Two++;
                }
                else
                {
                    RestoreCraftingItemsAfterFail(tempList,tempStack);
                    return;
                }           
            }
            tempList.Add(tempStack);
        }
        systems.statManager.harvestStrength = selectedCraftable.Two.value;
        systems.inventoryManager.currentPickaxe = selectedCraftable.Two;
    }

    private void RestoreCraftingItemsAfterFail(List<ItemStack> tempList, ItemStack tempStack)
    {
        // old stored item recovery
        if (tempList.Count > 0)
        {
            foreach (ItemStack tempListItemStack in tempList)
            {
                for (int k = 0; k < tempListItemStack.Two; k++)
                {
                    systems.inventoryManager.Add(tempListItemStack.One);
                }
            }
        }

        for (int j = 0; j < tempStack.Two; j++)
        {
            systems.inventoryManager.Add(tempStack.One);
        }
    }
}
