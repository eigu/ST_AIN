using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class InputEvents 
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<bool> OnJumpEvent;
    public event Action<bool> OnSprintEvent;
    public event Action<bool> OnCrouchEvent;
    public event Action OnPrimaryInteractEvent;
    public event Action OnSecondaryInteractEvent;
    public event Action OnSwitchViewEvent;
    public event Action OnShootEvent;
    public event Action<bool> OnAimEvent;
    public event Action<bool> OnCursorEvent;
    public event Action OnOpenInventoryEvent;
    public event Action OnCloseInventoryEvent;
    public event Action OnResumeEvent;
    public event Action OnPauseEvent;
    
    public event Action OnSetUIEvent;
    public event Action OnSetGameEvent;
    public event Action OnOpenMapEvent;

    public event Action OnSelectStartEvent;
    
    public event Action OnSelectEndEvent;

    public event Action<float> OnUIScrollEvent;
    public event Action<Vector2> OnGamepadPanEvent;
    public event Action<Vector2> OnMousePanEvent;

    public void Move(Vector2 input) => OnMoveEvent?.Invoke(input);
    public void Look(Vector2 input) => OnLookEvent?.Invoke(input);
    public void Jump(bool pressed) => OnJumpEvent?.Invoke(pressed);
    public void Sprint(bool pressed) => OnSprintEvent?.Invoke(pressed);
    public void Crouch(bool pressed) => OnCrouchEvent?.Invoke(pressed);
    public void PrimaryInteract() => OnPrimaryInteractEvent?.Invoke();
    
    public void SecondaryInteract() => OnSecondaryInteractEvent?.Invoke();
    public void SwitchView() => OnSwitchViewEvent?.Invoke();
    public void Shoot() => OnShootEvent?.Invoke();
    public void Aim(bool pressed) => OnAimEvent?.Invoke(pressed);
    public void OpenInventory() => OnOpenInventoryEvent?.Invoke();
    public void OpenMap() => OnOpenMapEvent?.Invoke();
    public void CloseInventory() => OnCloseInventoryEvent?.Invoke();
    public void Pause() => OnPauseEvent?.Invoke();
    public void Resume() => OnResumeEvent?.Invoke();
    public void Cursor(bool pressed) => OnCursorEvent?.Invoke(pressed);
    
    public void SetUI() => OnSetUIEvent?.Invoke();
    
    public void SetGame() => OnSetGameEvent?.Invoke();
    
    public void UIScroll(float val) => OnUIScrollEvent?.Invoke(val);

    public void OnSelectStart() => OnSelectStartEvent?.Invoke();
    public void OnSelectEnd() => OnSelectEndEvent?.Invoke();

    public void OnGamepadPan(Vector2 input) => OnGamepadPanEvent?.Invoke(input);
    public void OnMousePan(Vector2 input) => OnMousePanEvent?.Invoke(input);
}
