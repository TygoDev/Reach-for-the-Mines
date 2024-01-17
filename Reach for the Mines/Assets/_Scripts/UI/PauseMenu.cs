using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private void Start()
    {
        Systems.Instance.inputManager.pauseEvent += PauseTrue;
        Systems.Instance.inputManager.unPauseEvent += PauseFalse;
    }

    private void OnDisable()
    {
        Systems.Instance.inputManager.pauseEvent -= PauseTrue;
        Systems.Instance.inputManager.unPauseEvent -= PauseFalse;
    }

    public void UnStuck()
    {
        EventBus<UnStuckEvent>.Publish(new UnStuckEvent());
    }

    public void UnPause()
    {
        PauseFalse();
        Systems.Instance.stateManager.UpdateGameState(GameState.Gameplay);
    }

    #region event listeners
    void PauseTrue()
    {
        GetComponent<Canvas>().enabled = true;
    }

    void PauseFalse()
    {
        GetComponent<Canvas>().enabled = false;
    }
    #endregion
}
