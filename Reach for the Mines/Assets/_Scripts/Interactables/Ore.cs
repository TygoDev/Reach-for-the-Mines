using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Ore : MonoBehaviour
{
    public float health = 10f;
    public Canvas healthBarCanvas = null;
    public bool currentlyHitting = false;
    public event UnityAction OreDestroyed = delegate { };

    [SerializeField] private List<Drop> possibleDrops = new List<Drop>();
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private List<GameObject> itemPrefabModels = new List<GameObject>();
    [SerializeField] private int amountToDrop = 4;
    [SerializeField] private Slider healthSlider = null;

    private float damage = 0;
    private Camera mainCamera = null;

    private void Start()
    {
        mainCamera = Camera.main;
        healthSlider.maxValue = health;
        healthSlider.value = health;
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

    public void SetHealthbar(bool value)
    {
        if (healthBarCanvas.gameObject.activeInHierarchy != value)
        {
            healthBarCanvas.gameObject.SetActive(value);
        }
    }

    private void Harvest()
    {
        if (possibleDrops.Count != 0)
        {
            for (int i = 0; i < amountToDrop; i++)
            {
                Item randomItem = GetRandomItem();

                foreach (GameObject dropPrefab in itemPrefabModels)
                {
                    if(randomItem.name == dropPrefab.name)
                    {
                        itemPrefab = dropPrefab;
                        itemPrefab.GetComponent<ItemController>().item = randomItem;
                        break;
                    }
                }

                Vector3 randomPosition = new Vector3(
                    Random.Range(transform.position.x - 1, transform.position.x + 1),
                    transform.position.y,
                    Random.Range(transform.position.z - 1, transform.position.z + 1)
                );

                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);

                Instantiate(itemPrefab, randomPosition, randomRotation);
            }

            OreDestroyed.Invoke();
            EventBus<MinedEvent>.Publish(new MinedEvent());
            Destroy(gameObject);
        }
    }

    private Item GetRandomItem()
    {
        int totalWeight = 0;
        foreach (Drop drop in possibleDrops)
        {
            totalWeight += drop.Two;
        }

        int randomWeightValue = Random.Range(1, totalWeight + 1);

        int processedValue = 0;

        foreach (Drop drop in possibleDrops)
        {
            processedValue += drop.Two;

            if (randomWeightValue <= processedValue)
            {
                return drop.One;
            }
        }

        return null;
    }
}
