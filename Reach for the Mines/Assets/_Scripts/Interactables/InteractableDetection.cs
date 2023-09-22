using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableDetection : MonoBehaviour
{
    private const string INTERACTABLE = "Interactable";
    private Systems systems = null;
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable activeInteractable = null;

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
        if (activeInteractable != null)
        {
            interactables.Remove(activeInteractable);

            activeInteractable.Hit(systems.statManager.harvestStrength);
        }

        if (activeInteractable == null && interactables.Count > 0)
        {
            activeInteractable = interactables[interactables.Count - 1];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(INTERACTABLE))
        {
            interactables.Add(other.GetComponent<Interactable>());
            foreach (Interactable item in interactables)
            {
                item.SetInteractable(true);
            }

            activeInteractable = interactables[interactables.Count - 1];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(INTERACTABLE) && interactables.Contains(other.GetComponent<Interactable>()))
        {
            interactables.Remove(other.GetComponent<Interactable>());
            other.GetComponent<Interactable>().SetInteractable(false);

            if (interactables.Count > 0)
                activeInteractable = interactables[interactables.Count - 1];
        }

        if (other.CompareTag(INTERACTABLE) && other.GetComponent<Interactable>() == activeInteractable)
        {
            activeInteractable.SetInteractable(false);
            activeInteractable = null;
        }
    }
}
