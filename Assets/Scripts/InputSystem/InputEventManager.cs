using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Event Manager", menuName = "Input/Input Event Manager")]
public class InputEventManager : ScriptableObject
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action<bool> OnJumpEvent;
    public event Action<bool> OnSprintEvent;
    public event Action<bool> OnCrouchEvent;
    public event Action OnInteractEvent;
    public event Action OnSwitchViewEvent;
    public event Action OnShootEvent;
    public event Action<bool> OnAimEvent;
    public event Action<bool> OnCursorEvent;
    public event Action OnOpenInventoryEvent;
    public event Action OnCloseInventoryEvent;
    public event Action OnResumeEvent;
    public event Action OnPauseEvent;

    public void Move(Vector2 input) => OnMoveEvent?.Invoke(input);
    public void Look(Vector2 input) => OnLookEvent?.Invoke(input);
    public void Jump(bool pressed) => OnJumpEvent?.Invoke(pressed);
    public void Sprint(bool pressed) => OnSprintEvent?.Invoke(pressed);
    public void Crouch(bool pressed) => OnCrouchEvent?.Invoke(pressed);
    public void Interact() => OnInteractEvent?.Invoke();
    public void SwitchView() => OnSwitchViewEvent?.Invoke();
    public void Shoot() => OnShootEvent?.Invoke();
    public void Aim(bool pressed) => OnAimEvent?.Invoke(pressed);
    public void OpenInventory() => OnOpenInventoryEvent?.Invoke();
    public void CloseInventory() => OnCloseInventoryEvent?.Invoke();
    public void Pause() => OnPauseEvent?.Invoke();
    public void Resume() => OnResumeEvent?.Invoke();
    public void Cursor(bool pressed) => OnCursorEvent?.Invoke(pressed);
}