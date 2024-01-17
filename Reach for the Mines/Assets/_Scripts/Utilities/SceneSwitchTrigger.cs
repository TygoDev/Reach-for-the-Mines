using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneSwitchTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "";
    [SerializeField] private Button confirmButton = null;
    [SerializeField] private TMP_Text confirmMenuText = null;
    [SerializeField] private bool switchInstantly = true;
    [SerializeField] private MenuTrigger menuTrigger = null;
    [SerializeField] private float cost = 0;
    [SerializeField] private string textToSay = null;
    [SerializeField] private Vector3 spawnpoint = new Vector3(0, 1, 0);

    private const string PLAYER = "Player";
    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER) && switchInstantly)
        {
            LoadScene();
        }
        else if (other.CompareTag(PLAYER))
        {
            confirmMenuText.text = textToSay;
            menuTrigger.ToggleMenu(true);
            confirmButton.onClick.AddListener(LoadScene);

        }
    }

    private void LoadScene()
    {
        if(systems.statManager.goldAmount >= cost)
        {
            systems.statManager.goldAmount -= cost;
            systems.spawnpoint = spawnpoint;
            systems.stateManager.UpdateGameState(GameState.Gameplay);
            EventBus<SceneSwitchedEvent>.Publish(new SceneSwitchedEvent(sceneToLoad));
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
