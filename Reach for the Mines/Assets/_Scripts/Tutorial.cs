using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText = null;
    [SerializeField] private TMP_Text pressEnterText = null;
    [SerializeField] private List<string> tutorialLines = new List<string>();

    private int tutorialIndex = 0;
    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

        if (systems.tutorialComplete)
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    private void Initialize()
    {
        UpdateTutorialText();
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.enterEvent += FirstAdvancement;
    }

    private void Unsubscribe()
    {
        if (systems == null)
            return;

        systems.inputManager.enterEvent -= FirstAdvancement;
        systems.inputManager.mouseRotateEvent -= MouseMoved;
        systems.inputManager.moveEvent -= CharacterMoved;
        systems.inputManager.sprintCanceledEvent -= CharacterSprinted;
        systems.inputManager.jumpEvent -= CharacterJumped;
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    #region Event Listeners
    private void FirstAdvancement()
    {
        NextTutorial();
        TogglePressEnterText(false);

        Unsubscribe();
        systems.inputManager.mouseRotateEvent += MouseMoved;
    }

    private void MouseMoved(Vector2 x)
    {
        NextTutorial();
        Unsubscribe();

        systems.inputManager.moveEvent += CharacterMoved;
    }

    private void CharacterMoved(Vector2 x)
    {
        NextTutorial();
        Unsubscribe();

        systems.inputManager.sprintCanceledEvent += CharacterSprinted;
    }

    private void CharacterSprinted()
    {
        NextTutorial();
        Unsubscribe();

        systems.inputManager.jumpEvent += CharacterJumped;
    }

    private void CharacterJumped()
    {
        NextTutorial();
        Unsubscribe();

        SceneManager.activeSceneChanged += SwitchedToHUBworld;
    }

    private void SwitchedToHUBworld(Scene currentScene, Scene nextScene)
    {
        if (nextScene.name == "Hub World")
            NextTutorial();
    }
    #endregion

    private void NextTutorial()
    {
        if (tutorialIndex + 1 < tutorialLines.Count)
        {
            tutorialIndex++;
            UpdateTutorialText();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateTutorialText()
    {
        tutorialText.text = tutorialLines[tutorialIndex];
    }

    private void TogglePressEnterText(bool active)
    {
        pressEnterText.gameObject.SetActive(active);
    }
}
