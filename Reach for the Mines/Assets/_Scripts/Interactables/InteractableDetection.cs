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
            systems.stateManager.UpdateGameState(GameState.Menu);
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
            item.SetInteractable(true);
        }

        activeInteractable = interactables[interactables.Count - 1];
    }

    private void UnSetInteractable(Collider other)
    {
        if (interactables.Contains(other.GetComponent<Interactable>()))
        {
            interactables.Remove(other.GetComponent<Interactable>());
            other.GetComponent<Interactable>().SetInteractable(false);

            if (interactables.Count > 0)
                activeInteractable = interactables[interactables.Count - 1];
        }

        if (other.GetComponent<Interactable>() == activeInteractable)
        {
            activeInteractable.SetInteractable(false);
            activeInteractable = null;
        }
    }

    private void SetMenuTrigger(Collider other)
    {
        menuTrigger = other.GetComponent<MenuTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MENUTRIGGER))
        {
            SetMenuTrigger(other);
        }

        if (other.CompareTag(INTERACTABLE))
        {
            SetInteractable(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag(INTERACTABLE))
        {
            UnSetInteractable(other);
        }
    }
}
