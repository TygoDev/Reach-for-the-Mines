using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public new string name;
    public string description;
    public int value;
    public Sprite icon;
    public ItemType itemType;
}

public enum ItemType
{
    Default,
    Smeltable,
    Fuel,
    Station
}
