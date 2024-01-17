using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ore))]
public class XPSource : MonoBehaviour
{
    [SerializeField] private Ore ore = null;
    [SerializeField] private float xpValue = 0;

    private void OnEnable()
    {
        ore.OreDestroyed += AddXP;
    }

    private void OnDisable()
    {
        ore.OreDestroyed -= AddXP;
    }

    private void AddXP()
    {
        Systems.Instance.statManager.XPAmount += xpValue;
        Systems.Instance.statManager.level = (int)(0.25f * Mathf.Sqrt(Systems.Instance.statManager.XPAmount));
        Systems.Instance.xpGainedEvent.Invoke();
    }
}
