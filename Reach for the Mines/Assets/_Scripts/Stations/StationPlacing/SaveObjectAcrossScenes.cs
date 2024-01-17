using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveObjectAcrossScenes : MonoBehaviour
{
    public string targetSceneName = "Personal Plot";

    private static SaveObjectAcrossScenes instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != targetSceneName)
        {
            SetChildrenActiveState(false);
        }
        else
        {
            SetChildrenActiveState(true);
        }
    }

    private void SetChildrenActiveState(bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
