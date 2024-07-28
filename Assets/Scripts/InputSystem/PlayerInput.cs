using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input System/Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IPlayerActions, InputActions.IPauseMenuActions
{

    //public event UnityAction onMove;
    public event UnityAction<Vector2> onMove = delegate{};

    public event UnityAction onStopMove = delegate{};

    public event UnityAction onAttack = delegate{};
    public event UnityAction onStopAttack = delegate{};

    public event UnityAction onDodge = delegate{};

    public event UnityAction onPowerDrive = delegate{};

    public event UnityAction onPause = delegate{};

    public event UnityAction onUnPause = delegate{};
    InputActions inputActions;

    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
    }


    void OnDisable()
    {
        DisableAllInputs();
        //DisablePlayerInput();
    }
    public void DisablePlayerInput()
    {
        // inputActions.Player.Disable();
        // inputActions.PauseMenu.Disable();
        inputActions.Disable();
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
    }

    public void DisableAllInputs()
    {
        inputActions.Disable();
    }
    public void EnablePlayerInput()
    {
        // inputActions.Player.Enable();
        // inputActions.PauseMenu.Enable();
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        SwitchActionMap(inputActions.Player, false);
    }


    void SwitchActionMap(InputActionMap map, bool isShowCursor)
    {
        inputActions.Disable();
        map.Enable();

        if (isShowCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void EnablePauseMenuInput()
    {
        SwitchActionMap(inputActions.PauseMenu, true);
    }


    //修复按下tab键游戏卡死问题
    public void SwitchToDynamicUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }
    public void SwitchToFixedUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //onMove?.Invoke();
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //onMove?.Invoke();
            onAttack.Invoke();
        }
        if (context.canceled)
        {
            onStopAttack.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    public void OnPowerDrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPowerDrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }
}
