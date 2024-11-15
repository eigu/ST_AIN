using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, PlayerInputActions.IPlayerActions, PlayerInputActions.IUIActions
{
    private PlayerInputActions _playerInputActions;

    private void OnEnable()
    {
        if (_playerInputActions == null)
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.SetCallbacks(this);
            _playerInputActions.UI.SetCallbacks(this);
            SetGameplay();
        }

        GameEventsManager.Instance.InputEvents.OnSetGameEvent += SetGameplay;
        GameEventsManager.Instance.InputEvents.OnSetUIEvent += SetUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InputEvents.OnSetGameEvent -= SetGameplay;
        GameEventsManager.Instance.InputEvents.OnSetUIEvent -= SetUI;
    }

    private void SetGameplay()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.UI.Disable();
    }
    
    private void SetUI()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.UI.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        GameEventsManager.Instance.InputEvents.Move(context.ReadValue<Vector2>());
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.OpenInventory();
            SetUI();
        }
    }
    
    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.CloseInventory();
            SetGameplay();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        GameEventsManager.Instance.InputEvents.Look(context.ReadValue<Vector2>());
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Jump(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.InputEvents.Jump(false);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Crouch(true);
        }
        
        if (context.phase == InputActionPhase.Performed)
        {
            GameEventsManager.Instance.InputEvents.Crouch(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.InputEvents.Crouch(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.PrimaryInteract();
        }
    }

    public void OnInteractSecondary(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.SecondaryInteract();
        }
    }

    public void OnCursor(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Cursor(true);
        }
        
        if (context.phase == InputActionPhase.Performed)
        {
            GameEventsManager.Instance.InputEvents.Cursor(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.InputEvents.Cursor(false);
        }
    }

    public void OnSwitchView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.SwitchView();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Aim(true);
        }
        
        if (context.phase == InputActionPhase.Performed)
        {
            GameEventsManager.Instance.InputEvents.Aim(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.InputEvents.Aim(false);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Shoot();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Sprint(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            GameEventsManager.Instance.InputEvents.Sprint(false);
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Resume();
            SetGameplay();
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            GameEventsManager.Instance.InputEvents.Pause();
            SetUI();
        }
    }

    public void OnNavigation(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }
}