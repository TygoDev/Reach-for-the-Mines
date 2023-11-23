using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    public bool occupied = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MenuTrigger"))
        {
            occupied = true;
        }

    }
}
