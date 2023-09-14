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

    public event UnityAction pauseEvent = delegate { };

    //menu
    public event UnityAction unpauseEvent = delegate { };

    private GameInput gameInput;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Menu.SetCallbacks(this);
            gameInput.Gameplay.SetCallbacks(this);

        }

        EnableGameplayInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
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
        DisableAllInput();

        EnableGameplayInput();
        unpauseEvent.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        DisableAllInput();

        EnableMenuInput();
        pauseEvent.Invoke();
    }

    public void OnMouseRotation(InputAction.CallbackContext context)
    {
        mouseRotateEvent.Invoke(context.ReadValue<Vector2>());
    }
}
