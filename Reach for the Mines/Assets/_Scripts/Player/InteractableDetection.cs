using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableDetection : MonoBehaviour
{
    private const string ORE = "Ore";
    private const string MENUTRIGGER = "MenuTrigger";

    private List<Ore> ores = new List<Ore>();
    private Ore activeOre = null;

    private MenuTrigger menuTrigger = null;

    private void Start()
    {
        Systems.Instance.inputManager.hitEvent += Hit;
        Systems.Instance.inputManager.interactEvent += Interact;
    }

    private void OnDisable()
    {
        Systems.Instance.inputManager.hitEvent -= Hit;
        Systems.Instance.inputManager.interactEvent -= Interact;
    }

    private void Interact()
    {
        if(menuTrigger != null)
            menuTrigger.ToggleMenu(true);
    }

    private void Hit()
    {
        if (activeOre != null)
        {
            ores.Remove(activeOre);
            activeOre.Hit(Systems.Instance.statManager.harvestStrength);
        }

        SetNewOreInOrder();

    }

    private void SetNewOreInOrder()
    {
        if (activeOre == null && ores.Count > 0)
        {
            activeOre = ores[ores.Count - 1];
        }
    }

    private void SetInteractable(Collider other)
    {
        ores.Add(other.GetComponent<Ore>());

        foreach (Ore item in ores)
        {
            item.SetHealthbar(true);
        }

        SetNewOreInOrder();
    }

    private void UnSetInteractable(Collider other)
    {
        Ore ore = other.GetComponent<Ore>();

        if (ores.Contains(ore))
        {
            ores.Remove(ore);
            ore.SetHealthbar(false);

            SetNewOreInOrder();
        }

        if (ore == activeOre)
        {
            activeOre.SetHealthbar(false);
            activeOre = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MENUTRIGGER))
        {
            menuTrigger = other.GetComponent<MenuTrigger>();
        }

        if (other.CompareTag(ORE))
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

        if (other.CompareTag(ORE))
        {
            UnSetInteractable(other);
        }
    }
}
