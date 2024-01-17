using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item = null;
    private const string PLAYER = "Player";
    GameObject target = null;
    private float attractionForce = 10f;
    private float maxAttractionRange = 5f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(PLAYER);
    }

    private void PickUp()
    {
        Systems.Instance.inventoryManager.Add(item, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER))
            PickUp();
    }

    private void Update()
    {
        Magnetize();
    }

    private void Magnetize()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > 0 && distanceToTarget <= maxAttractionRange)
        {
            float force = Mathf.Clamp(1 / distanceToTarget, 0f, 1f) * attractionForce;
            Vector3 normalizedDirection = directionToTarget.normalized;
            transform.Translate(normalizedDirection * force * Time.deltaTime, Space.World);
        }
    }

}
