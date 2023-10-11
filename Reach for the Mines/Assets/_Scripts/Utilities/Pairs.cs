using System;
using System.Collections.Generic;
using Utils;

[Serializable]
public class ItemStack : Pair<Item, int>
{
    public ItemStack(Item item, int quantity) : base(item, quantity) { }
}

[Serializable]
public class Craftable : Pair<List<ItemStack>,Item>
{
    public Craftable(List<ItemStack> recipe, Item result) : base(recipe, result) { }
}

[Serializable]
public class Smeltable : Pair<Item,Item>
{
    public Smeltable(Item itemInput, Item itemOutput) : base(itemInput, itemOutput) { }
}

[Serializable]
public class Drop : Pair<Item,int>
{
    public Drop(Item item, int rarity) : base(item, rarity) { }
}
