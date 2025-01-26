using System;
using ScriptableObjectArchitecture;
using UnityEngine;

public class UIEvents
{
    public event Action<bool> OnTogglePauseMenuEvent; // can be used for freezing
    public event Action<string> OnUpdateUIGuideTextEvent;
    public event Action<UIPanelBase> OnOpenUIPanelEvent;
    public event Action OnQuitButtonPressedEvent;
    
    //object holder, MapIconDataSO, should it be highlighted?, overrideScaleMultiplier
    public event Action<Transform, MapIconDataSO, string, bool, float> RegisterMapWorldObjectIconEvent;
    public event Action<Transform> UnregisterMapWorldObjectIconEvent;

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
    
    public void RegisterMapWorldObjectIcon(Transform objRef, MapIconDataSO iconInfo, string overrideName, bool isHighlighted, float overrideScaleMultiplier)
    {
        RegisterMapWorldObjectIconEvent?.Invoke(objRef, iconInfo, overrideName, isHighlighted, overrideScaleMultiplier);
    }
    
    public void UnregisterMapWorldObjectIcon(Transform objRef)
    {
        UnregisterMapWorldObjectIconEvent?.Invoke(objRef);
    }
}