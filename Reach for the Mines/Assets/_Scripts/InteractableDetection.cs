using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetection : MonoBehaviour
{
    private const string INTERACTABLE = "Interactable";
    private Systems systems = null;
    private Interactable interactable = null;

    private void Start()
    {
        systems = Systems.Instance;
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.hitEvent += Hit;
    }

    private void OnDisable()
    {
        systems.inputManager.hitEvent -= Hit;
    }

    private void Hit()
    {
       if(interactable != null)
            interactable.Hit(systems.statManager.harvestStrength);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(INTERACTABLE))
        {
            interactable = other.GetComponent<Interactable>();
            interactable.SetHealthBarCanvas(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(INTERACTABLE))
        {
            interactable.SetHealthBarCanvas(false);
            interactable.currentlyHitting = false;
            interactable = null;
        }
    }
}
