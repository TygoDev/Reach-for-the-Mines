using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ore))]
public class XPSource : MonoBehaviour
{
    [SerializeField] private Ore ore = null;
    [SerializeField] private float xpValue = 0;

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

        Subscribe();
    }

    private void Subscribe()
    {
        ore.OreDestroyed += AddXP;
    }

    private void OnDisable()
    {
        ore.OreDestroyed -= AddXP;
    }

    private void AddXP()
    {
        systems.statManager.XPAmount += xpValue;
        systems.statManager.level = (int)(0.25f * Mathf.Sqrt(systems.statManager.XPAmount));
        systems.xpGainedEvent.Invoke();
    }
}
