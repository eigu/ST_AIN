using System;
using UnityEngine;

public class UIEvents
{
    public event Action<bool> OnTogglePauseMenuEvent; // can be used for freezing
    public event Action<string> OnUpdateUIGuideTextEvent;
    public event Action<UIPanelBase> OnOpenUIPanelEvent;
    public event Action OnQuitButtonPressedEvent;

    public void UpdateUIGuideText(string obj)
    {
        OnUpdateUIGuideTextEvent?.Invoke(obj);
    }
    
    public void OpenUIPanel(UIPanelBase uiPanel)
    {
        OnOpenUIPanelEvent?.Invoke(uiPanel);
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