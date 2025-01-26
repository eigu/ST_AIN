using System;
using UnityEngine;
using UnityEngine.Events;

public class UIPanel : UIPanelBase
{
    [SerializeField] private UnityEvent OnOpenPanelEvent;
    [SerializeField] private UnityEvent OnClosePanelEvent;
    public override void OnOpenPanel()
    {
        OnOpenPanelEvent?.Invoke();
    }

    public override void ClosePanel()
    {
        OnClosePanelEvent?.Invoke();
        gameObject.SetActive(false);
    }
}