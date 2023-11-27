using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Custom/Input Manager")]
public class InputManager : ScriptableObject, GameInput.IGameplayActions, GameInput.IMenuActions
{
    //gameplay
    public event UnityAction<Vector2> moveEvent = delegate { };
    public event UnityAction<Vector2> mouseRotateEvent = delegate { };
    public event UnityAction jumpEvent = delegate { };
    public event UnityAction sprintEvent = delegate { };
    public event UnityAction sprintCanceledEvent = delegate { };
    public event UnityAction openInventoryEvent = delegate { };
    public event UnityAction hitEvent = delegate { };
    public event UnityAction interactEvent = delegate { };

    //menu
    public event UnityAction unPauseEvent = delegate { };

    private GameInput gameInput;

    public StateManager stateManager;

    private bool hitActionInProgress = false;

    private void OnEnable()
    {

        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Menu.SetCallbacks(this);
            gameInput.Gameplay.SetCallbacks(this);
        }

        stateManager.onGameStateChanged += OnGameStateChanged;

        if (stateManager.GetGameState() == GameState.Gameplay)
            EnableGameplayInput();
        else
            EnableMenuInput();
    }

    private void OnDisable()
    {
        stateManager.onGameStateChanged -= OnGameStateChanged;
    }

    private void DisableAllInput()
    {
        gameInput.Gameplay.Disable();
        gameInput.Menu.Disable();
    }

    private void EnableGameplayInput()
    {
        gameInput.Gameplay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void EnableMenuInput()
    {
        gameInput.Menu.Enable();
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnPause()
    {
        unPauseEvent.Invoke();
        stateManager.UpdateGameState(GameState.Gameplay);
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            hitActionInProgress = true;
            moveEvent.Invoke(new Vector2(0, 0));
            hitEvent.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
            hitActionInProgress = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!hitActionInProgress)
            moveEvent.Invoke(context.ReadValue<Vector2>());
        else
            moveEvent.Invoke(new Vector2(0, 0));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!hitActionInProgress)
            jumpEvent.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            sprintEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            sprintCanceledEvent.Invoke();
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            stateManager.UpdateGameState(GameState.Gameplay);
            unPauseEvent.Invoke();
        }

    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
            stateManager.UpdateGameState(GameState.Menu);
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            stateManager.UpdateGameState(GameState.Menu);
            openInventoryEvent.Invoke();
        }

    }

    public void OnMouseRotation(InputAction.CallbackContext context)
    {
        mouseRotateEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            stateManager.UpdateGameState(GameState.Menu);
            interactEvent.Invoke();
        }
    }

    // EVENT LISTENERS

    public void OnGameStateChanged(GameState state)
    {
        DisableAllInput();

        if (state == GameState.Gameplay)
            EnableGameplayInput();
        else if (state == GameState.Menu)
            EnableMenuInput();
    }
}
