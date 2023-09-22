using UnityEngine;

[CreateAssetMenu(fileName = "StatManager", menuName = "Custom/Stat Manager")]
public class StatManager : ScriptableObject
{
    public float harvestStrength = 5;
    public int level = 0;
    public float XPAmount = 0;
}
