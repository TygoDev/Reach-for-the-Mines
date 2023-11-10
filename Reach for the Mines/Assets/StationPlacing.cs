using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlacing : MonoBehaviour
{
    public GameObject selectedStation;
    private GameObject currentStationInstance;
    private BuildingArea currentBuildingArea;
    private bool canPlaceStation = true;
    public float cooldownTime = 0.25f; // Adjust as needed

    void Update()
    {
        if (!canPlaceStation)
        {
            // If cooldown is active, reduce the cooldown time
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0f)
            {
                canPlaceStation = true;
                cooldownTime = 0.25f; // Reset the cooldown time
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

                    // Instantiate the station if it doesn't exist or if the building area changes
                    if (currentStationInstance == null || currentBuildingArea != hitBuildingArea)
                    {
                        DestroyStation(); // Destroy the previous station if any
                        currentStationInstance = Instantiate(selectedStation, new Vector3(centerOfBuildingArea.x, 0.5f, centerOfBuildingArea.z), Quaternion.identity, transform);
                        currentBuildingArea = hitBuildingArea;
                        currentBuildingArea.occupied = true;

                        canPlaceStation = false; // Set cooldown
                    }
                }
            }
            else
            {
                // If the ray doesn't hit the building area, destroy the instantiated station
                DestroyStation();
            }
        }
        else
        {
            // If the ray doesn't hit anything, destroy the instantiated station
            DestroyStation();
        }
    }

    // Function to destroy the instantiated station
    private void DestroyStation()
    {
        if (currentStationInstance != null)
        {
            currentBuildingArea.occupied = false; // Set occupied to false before destroying
            Destroy(currentStationInstance);
            currentStationInstance = null;
            currentBuildingArea = null;
            canPlaceStation = false; // Set cooldown
        }
    }
}
