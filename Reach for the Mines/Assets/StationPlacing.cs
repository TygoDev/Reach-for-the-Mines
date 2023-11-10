using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlacing : MonoBehaviour
{
    public GameObject selectedStation;
    private GameObject currentStationInstance;
    private Vector3 currentStationCenter;
    private BuildingArea currentBuildingArea;
    private bool canPlaceStation = true;
    public float cooldownTime = 0.25f;

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.hitEvent += Place;
        systems.inputManager.unPauseEvent += Disable;
    }

    private void OnEnable()
    {
        if (systems != null)
        {
            systems.inputManager.hitEvent += Place;
            systems.inputManager.unPauseEvent += Disable;
        }
    }

    private void OnDisable()
    {
        systems.inputManager.hitEvent -= Place;
        systems.inputManager.unPauseEvent -= Disable;
    }

    void Disable()
    {
        DestroyStation();
        canPlaceStation = true;

        enabled = false;
    }

    void Place()
    {
        if (currentStationInstance != null && currentBuildingArea != null)
        {
            currentStationInstance.transform.position = new Vector3(currentStationCenter.x, 0.5f, currentStationCenter.z);
            currentBuildingArea.occupied = true;

            currentBuildingArea = null;
            currentStationInstance = null;
            canPlaceStation = true;

            enabled = false;
        }
    }

    void Update()
    {
        if (!canPlaceStation)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0f)
            {
                canPlaceStation = true;
                cooldownTime = 0.25f;
            }
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        Ray ray = new Ray(cameraPosition, cameraForward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("BuildingArea"))
            {
                BuildingArea hitBuildingArea = hit.collider.gameObject.GetComponent<BuildingArea>();
                if (!hitBuildingArea.occupied && canPlaceStation)
                {
                    Vector3 centerOfBuildingArea = hit.collider.bounds.center;
                    currentStationCenter = centerOfBuildingArea;

                    if (currentStationInstance == null || currentBuildingArea != hitBuildingArea)
                    {
                        DestroyStation();
                        currentStationInstance = Instantiate(selectedStation, new Vector3(centerOfBuildingArea.x, 0.5f, centerOfBuildingArea.z), Quaternion.identity, transform);
                        currentBuildingArea = hitBuildingArea;
                        currentBuildingArea.occupied = true;

                        canPlaceStation = false;
                    }
                }
            }
            else
            {
                DestroyStation();
            }
        }
        else
        {
            DestroyStation();
        }
    }

    private void DestroyStation()
    {
        if (currentStationInstance != null)
        {
            currentBuildingArea.occupied = false;
            Destroy(currentStationInstance);
            currentStationInstance = null;
            currentBuildingArea = null;
            canPlaceStation = false;
        }
    }
}
