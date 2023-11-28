using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableDetection : MonoBehaviour
{
    private const string INTERACTABLE = "Interactable";
    private const string MENUTRIGGER = "MenuTrigger";

    private Systems systems = null;
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable activeInteractable = null;
    private MenuTrigger menuTrigger = null;

    private void Start()
    {
        systems = Systems.Instance;
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.hitEvent += Hit;
        systems.inputManager.interactEvent += Interact;
    }

    private void OnDisable()
    {
        systems.inputManager.hitEvent -= Hit;
        systems.inputManager.interactEvent -= Interact;
    }

    private void Interact()
    {
        if(menuTrigger != null)
        {
            menuTrigger.ToogleMenu(true);
        }
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

    private void SetInteractable(Collider other)
    {
        interactables.Add(other.GetComponent<Interactable>());
        foreach (Interactable item in interactables)
        {
            item.SetHealthbar(true);
        }

        activeInteractable = interactables[interactables.Count - 1];
    }

    private void UnSetInteractable(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactables.Contains(interactable))
        {
            interactables.Remove(interactable);
            interactable.SetHealthbar(false);

            if (interactables.Count > 0)
                activeInteractable = interactables[interactables.Count - 1];
        }

        if (interactable == activeInteractable)
        {
            activeInteractable.SetHealthbar(false);
            activeInteractable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MENUTRIGGER))
        {
            menuTrigger = other.GetComponent<MenuTrigger>();
        }

        if (other.CompareTag(INTERACTABLE))
        {
            SetInteractable(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(MENUTRIGGER))
        {
            menuTrigger = null;
        }

        if (other.CompareTag(INTERACTABLE))
        {
            UnSetInteractable(other);
        }
    }
}
