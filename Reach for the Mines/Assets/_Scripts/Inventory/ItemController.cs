using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item = null;

    private Systems systems = default;
    private const string PLAYER = "Player";

    private void Start()
    {
        systems = Systems.Instance;
    }

    private void Awake()
    {
        systems = Systems.Instance;
    }

    private void PickUp()
    {
        systems.inventoryManager.Add(item, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER))
            PickUp();
    }
}
