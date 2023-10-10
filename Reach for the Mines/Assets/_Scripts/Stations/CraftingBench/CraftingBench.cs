using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

[RequireComponent(typeof(CraftingBenchUI))]
public class CraftingBench : MonoBehaviour
{
    public Craftable selectedCraftable = null;
    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;
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
                }
                else
                {
                    RestoreCraftingItemsAfterFail(tempList,tempStack);
                    return;
                }           
            }
            tempList.Add(tempStack);
        }
        systems.inventoryManager.Add(selectedCraftable.result);
    }

    private void RestoreCraftingItemsAfterFail(List<ItemStack> tempList, ItemStack tempStack)
    {
        // old stored item recovery
        if (tempList.Count > 0)
        {
            foreach (ItemStack tempListItemStack in tempList)
            {
                for (int k = 0; k < tempListItemStack.quantity; k++)
                {
                    systems.inventoryManager.Add(tempListItemStack.item);
                }
            }
        }

        for (int j = 0; j < tempStack.quantity; j++)
        {
            systems.inventoryManager.Add(tempStack.item);
        }
    }
}
