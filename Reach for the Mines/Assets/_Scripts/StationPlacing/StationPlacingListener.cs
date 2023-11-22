using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlacingListener : MonoBehaviour
{
    private Systems systems = null;
    [SerializeField] private StationPlacing stationPlacing = null;


    private void Start()
    {
        systems = Systems.Instance;

        Subscribe();
    }

    void Subscribe()
    {
        systems.stationClickedEvent += EnableStationPlacing;
    }

    void EnableStationPlacing(Item item)
    {
        stationPlacing.Enable(item);
    }
}
