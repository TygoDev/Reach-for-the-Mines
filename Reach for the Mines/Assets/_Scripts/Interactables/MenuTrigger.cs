using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    private Canvas menuToTrigger = null;

    private void Start()
    {
        menuToTrigger = GetComponentInChildren<Canvas>();
    }

    public void ToogleMenu(bool value)
    {
        menuToTrigger.GetComponent<Canvas>().enabled = value;
    }
}
