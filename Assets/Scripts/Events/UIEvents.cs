using System;
using UnityEngine;

public class UIEvents
{
    public event Action<bool> OnTogglePauseMenuEvent;
    public event Action<string> OnUpdateUIGuideTextEvent;
    public event Action<GameObject> OnOpenUIPanelEvent;
    public event Action OnQuitButtonPressedEvent;

    public void UpdateUIGuideText(string obj)
    {
        OnUpdateUIGuideTextEvent?.Invoke(obj);
    }
    
    public void OpenUIPanel(GameObject obj)
    {
        OnOpenUIPanelEvent?.Invoke(obj);
    }
    
    public void TogglePauseMenu(bool obj)
    {
        OnTogglePauseMenuEvent?.Invoke(obj);
    }

    public void QuitButtonPressed()
    {
        OnQuitButtonPressedEvent?.Invoke();
    }
}