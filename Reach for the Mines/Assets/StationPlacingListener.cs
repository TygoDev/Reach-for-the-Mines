using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlacingListener : MonoBehaviour
{
    private Systems systems = null;
    private StationPlacing stationPlacing = null;


    private void Start()
    {
        stationPlacing = GetComponent<StationPlacing>();
        systems = Systems.Instance;

        Subscribe();
    }

    void Subscribe()
    {
        systems.stationClickedEvent += EnableStationPlacing;
    }

    void EnableStationPlacing(Item item)
    {
        stationPlacing.enabled = true;
        stationPlacing.Enable(item);
    }
}
