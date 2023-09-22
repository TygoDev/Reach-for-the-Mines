using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private List<Drop> possibleDrops = new List<Drop>();
    [SerializeField] private ItemController itemPrefab = null;
    [SerializeField] private int amountToDrop = 4;
    [SerializeField] private Slider healthSlider = null;

    public float health = 10f;
    public Canvas healthBarCanvas = null;
    public bool currentlyHitting = false;

    private float damage = 0;
    private Camera mainCamera = null;

    public event UnityAction InteractableDestroyed = delegate { };

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (healthBarCanvas.gameObject.activeInHierarchy)
            healthBarCanvas.transform.LookAt(mainCamera.transform.position);

        if (currentlyHitting && health > 0)
        {
            health -= Time.deltaTime * damage;
            healthSlider.value = health;

        }
        else if (health <= 0)
        {
            Harvest();
        }
    }

    public void Hit(float pDamage)
    {
        damage = pDamage;
        currentlyHitting = !currentlyHitting;
    }

    public void SetInteractable(bool value)
    {
        if (healthBarCanvas.gameObject.activeInHierarchy != value)
            healthBarCanvas.gameObject.SetActive(value);
    }

    private void Harvest()
    {
        if (possibleDrops.Count != 0)
        {
            for (int i = 0; i < amountToDrop; i++)
            {
                itemPrefab.item = GetRandomItem();
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }

            InteractableDestroyed.Invoke();
            Destroy(gameObject);
        }
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

            if (randomWeightValue <= processedValue)
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
