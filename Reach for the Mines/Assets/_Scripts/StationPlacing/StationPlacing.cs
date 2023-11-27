using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPlacing : MonoBehaviour
{
    public List<GameObject> stations = new List<GameObject>();
    public Item currentItem;
    public GameObject selectedStation;
    private GameObject currentStationInstance;
    private Vector3 currentStationCenter;
    private BuildingArea currentBuildingArea;
    private bool canPlaceStation = true;
    private bool placingEnabled = false;
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

    public void Enable(Item pSelectedStation)
    {
        placingEnabled = true;
        currentItem = pSelectedStation;
        foreach (GameObject item in stations)
        {
            if (item.name == pSelectedStation.name)
                selectedStation = item;
        }
    }

    void Disable()
    {
        DestroyStation();
        canPlaceStation = true;
        selectedStation = null;
        placingEnabled = false;
    }

    void Place()
    {
        if (currentStationInstance != null && currentBuildingArea != null && placingEnabled)
        {
            currentStationInstance.transform.position = new Vector3(currentStationCenter.x, 0.5f, currentStationCenter.z);
            currentBuildingArea.occupied = true;

            currentBuildingArea = null;
            currentStationInstance = null;
            canPlaceStation = true;
            systems.inventoryManager.Remove(currentItem);
            currentItem = null;

            Disable();
        }
    }

    void Update()
    {
        if (placingEnabled)
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

                        // Check if the currentStationInstance needs to be re-initialized
                        if (currentStationInstance == null || currentBuildingArea != hitBuildingArea)
                        {
                            DestroyStation();
                            currentBuildingArea = hitBuildingArea;
                            // Instantiate the new station and assign it to currentStationInstance
                            currentStationInstance = Instantiate(selectedStation, new Vector3(centerOfBuildingArea.x, 0.5f, centerOfBuildingArea.z), Quaternion.identity, transform);
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
    }


    private void DestroyStation()
    {
        if (currentStationInstance != null)
        {
            Destroy(currentStationInstance);
            currentStationInstance = null;
            currentBuildingArea = null;
            canPlaceStation = false;
        }
    }
}
