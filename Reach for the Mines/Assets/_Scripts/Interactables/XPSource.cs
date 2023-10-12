using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class XPSource : MonoBehaviour
{
    [SerializeField] private Interactable interactable = null;
    [SerializeField] private float xpValue = 0;

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

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
        systems.xpGainedEvent.Invoke();
    }
}
