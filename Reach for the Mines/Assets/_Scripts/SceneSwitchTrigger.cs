using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "";

    private const string PLAYER = "Player";
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
            SceneManager.LoadScene(sceneToLoad);


    }
}
