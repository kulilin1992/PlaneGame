using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input System/Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IPlayerActions
{

    //public event UnityAction onMove;
    public event UnityAction<Vector2> onMove = delegate{};

    public event UnityAction onStopMove = delegate{};
    InputActions inputActions;

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
    }


    void OnDisable()
    {
        DisablePlayerInput();
    }
    public void DisablePlayerInput()
    {
        inputActions.Player.Disable();
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
    }
    public void EnablePlayerInput()
    {
        inputActions.Player.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //onMove?.Invoke();
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }
}
