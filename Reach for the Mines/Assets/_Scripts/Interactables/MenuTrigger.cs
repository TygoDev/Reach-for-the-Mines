using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    [SerializeField] private Canvas menuToTrigger = null;

    public void ToogleMenu(bool value)
    {
        menuToTrigger.GetComponent<Canvas>().enabled = value;
    }
}
