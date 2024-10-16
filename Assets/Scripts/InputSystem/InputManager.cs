using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, PlayerInputActions.IPlayerActions, PlayerInputActions.IUIActions
{
    [Header("Input Event Manager")]
    public InputEventManager inputEventManager;

    private PlayerInputActions _playerInputActions;
    
    //DDOL
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    private void OnEnable()
    {
        if (_playerInputActions == null)
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.SetCallbacks(this);
            _playerInputActions.UI.SetCallbacks(this);
            SetGameplay();
        }
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
        inputEventManager.Move(context.ReadValue<Vector2>());
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.OpenInventory();
            SetUI();
        }
    }
    
    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.CloseInventory();
            SetGameplay();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        inputEventManager.Look(context.ReadValue<Vector2>());
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Jump(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            inputEventManager.Jump(false);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Crouch(true);
        }
        
        if (context.phase == InputActionPhase.Performed)
        {
            inputEventManager.Crouch(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            inputEventManager.Crouch(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Interact();
        }
    }

    public void OnCursor(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Cursor(true);
        }
        
        if (context.phase == InputActionPhase.Performed)
        {
            inputEventManager.Cursor(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            inputEventManager.Cursor(false);
        }
    }

    public void OnSwitchView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.SwitchView();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Aim(true);
        }
        
        if (context.phase == InputActionPhase.Performed)
        {
            inputEventManager.Aim(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            inputEventManager.Aim(false);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Shoot();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Sprint(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            inputEventManager.Sprint(false);
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Resume();
            SetGameplay();
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inputEventManager.Pause();
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