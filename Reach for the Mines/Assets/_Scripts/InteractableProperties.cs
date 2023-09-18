using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProperties : MonoBehaviour
{
    [SerializeField] private List<Drop> possibleDrops = new List<Drop>();
    [SerializeField] private ItemController itemPrefab = null;
    [SerializeField] private int amountToDrop;

    private void Start()
    {
        Mined();
    }

    private void Mined()
    {
        if(possibleDrops.Count != 0)
        {
            for (int i = 0; i < amountToDrop; i++)
            {
                itemPrefab.item = GetRandomItem();
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    private Item GetRandomItem()
    {
        int totalWeight = 0;
        foreach (Drop drop in possibleDrops)
        {
            totalWeight += drop.rarity;
        }

        int randomWeightValue = Random.Range(1, totalWeight + 1);

        int processedValue = 0;

        foreach (Drop drop in possibleDrops)
        {
            processedValue += drop.rarity;

            if(randomWeightValue <= processedValue)
            {
                return drop.item;
            }
        }

        return null;
    }
}

[System.Serializable]
public class Drop
{
    public Item item;
    public int rarity;

    public Drop(Item item, int rarity)
    {
        this.item = item;
        this.rarity = rarity;
    }
}
