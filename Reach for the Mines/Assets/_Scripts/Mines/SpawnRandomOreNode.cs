using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomOreNode : MonoBehaviour
{
    public List<Interactable> oreNodes = new List<Interactable>();

    private void Start()
    {
        int random = Random.Range(0,oreNodes.Count);

        Instantiate(oreNodes[random], transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(1,1,1));
    }
}
