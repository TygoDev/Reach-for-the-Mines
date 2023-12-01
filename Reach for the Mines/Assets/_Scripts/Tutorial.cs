using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<string> tutorialLines = new List<string>();
    [SerializeField] private TMP_Text tutorialText = null;

    public int tutorialIndex = 0;
    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

        if (systems.tutorialComplete)
            Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        tutorialText.text = tutorialLines[tutorialIndex];
        tutorialIndex++;
        systems.inputManager.enterEvent += NextTutorial;
    }

    private void OnDisable()
    {
        if(systems!=null)
        systems.inputManager.enterEvent -= NextTutorial;
    }

    private void NextTutorial()
    {
        if (tutorialIndex < tutorialLines.Count)
        {
            tutorialText.text = tutorialLines[tutorialIndex];
            tutorialIndex++;
        }
        else
        {
            systems.tutorialComplete = true;
            Destroy(gameObject);
        }
    }
}
