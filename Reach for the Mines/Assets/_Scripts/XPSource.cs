using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSource : MonoBehaviour
{
    private Systems systems = null;

    [SerializeField] private float xpValue = 0;

    private void Start()
    {
        systems = Systems.Instance;
    }

    private void OnDestroy()
    {
        systems.statManager.XPAmount += xpValue;
        systems.statManager.level = (int)(0.25f * Mathf.Sqrt(systems.statManager.XPAmount));
    }
}
