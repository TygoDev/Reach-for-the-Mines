using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSource : MonoBehaviour
{
    private Systems systems = null;
    private Interactable interactable = null;

    [SerializeField] private float xpValue = 0;

    private void Start()
    {
        systems = Systems.Instance;
        interactable = GetComponent<Interactable>();

        Subscribe();
    }

    private void Subscribe()
    {
        interactable.InteractableDestroyed += AddXP;
    }

    private void OnDisable()
    {
        interactable.InteractableDestroyed -= AddXP;
    }
    private void AddXP()
    {
        systems.statManager.XPAmount += xpValue;
        systems.statManager.level = (int)(0.25f * Mathf.Sqrt(systems.statManager.XPAmount));
    }
}
