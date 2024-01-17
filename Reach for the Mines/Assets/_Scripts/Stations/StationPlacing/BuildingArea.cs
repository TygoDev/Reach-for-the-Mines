using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    public bool occupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MenuTrigger"))
        {
            occupied = true;
            MenuTrigger menuTrigger = other.gameObject.GetComponent<MenuTrigger>();
            if (menuTrigger != null)
            {
                // Subscribe to the OnDestroy event
                menuTrigger.OnDestroyed += HandleMenuTriggerDestroyed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MenuTrigger"))
        {
            occupied = false;
        }
    }

    private void HandleMenuTriggerDestroyed()
    {
        // This method will be called when the MenuTrigger object is destroyed
        occupied = false;
    }
}
