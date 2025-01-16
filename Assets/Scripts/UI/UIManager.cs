using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _guideTMP;
    [SerializeField] private UIPanelBase pausePanelBase;
    private Stack<UIPanelBase> _openedPanel = new Stack<UIPanelBase>();

    private void OnEnable()
    {
        GameEventsManager.Instance.UIEvents.OnUpdateUIGuideTextEvent += UpdateUIGuideText;
        GameEventsManager.Instance.InputEvents.OnPauseEvent += ClosePanel;
        GameEventsManager.Instance.UIEvents.OnOpenUIPanelEvent += AddOpenedPanel;
        GameEventsManager.Instance.InputEvents.OnResumeEvent += ClosePanel;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.UIEvents.OnUpdateUIGuideTextEvent -= UpdateUIGuideText;
        GameEventsManager.Instance.InputEvents.OnPauseEvent -= ClosePanel;
        GameEventsManager.Instance.UIEvents.OnOpenUIPanelEvent -= AddOpenedPanel;
        GameEventsManager.Instance.InputEvents.OnResumeEvent -= ClosePanel;
    }

    private void UpdateUIGuideText(string text)
    {
        _guideTMP.text = text;
    }

    private void AddOpenedPanel(UIPanelBase obj)
    {
        obj.gameObject.SetActive(true);
        obj.OnOpenPanel();
        GameEventsManager.Instance.InputEvents.SetUI();
        _openedPanel.Push(obj);
    }

    public void ClosePanel()
    {
        Debug.Log("before: " + _openedPanel.Count);
        
        if (_openedPanel.Count <= 0)
        {
            OpenPanel(pausePanelBase);
        }
        else
        {
            _openedPanel.Pop().ClosePanel();
            
            if (_openedPanel.Count <= 0)
            {
                GameEventsManager.Instance.InputEvents.SetGame();
            }
            
        }

        Debug.Log("after: " + _openedPanel.Count);
    }

    public void OpenPanel(UIPanelBase obj)
    {
        AddOpenedPanel(obj);
    }

    public void ClosePausePanel()
    {
        ClosePanel();
    }

    public void OnQuitButton()
    {
        GameEventsManager.Instance.UIEvents.QuitButtonPressed();
    }
}
