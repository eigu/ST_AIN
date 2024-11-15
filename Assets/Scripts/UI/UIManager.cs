using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _guideTMP;
    [SerializeField] private GameObject _pausePanel;
    private Stack<GameObject> _openedPanel = new Stack<GameObject>();

    private void OnEnable()
    {
        GameEventsManager.Instance.UIEvents.OnUpdateUIGuideTextEvent += UpdateUIGuideText;
        GameEventsManager.Instance.InputEvents.OnPauseEvent += ClosePanel;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.UIEvents.OnUpdateUIGuideTextEvent -= UpdateUIGuideText;
        GameEventsManager.Instance.InputEvents.OnPauseEvent -= ClosePanel;
    }

    private void UpdateUIGuideText(string text)
    {
        _guideTMP.text = text;
    }

    private void AddOpenedPanel(GameObject obj)
    {
        _openedPanel.Push(obj);
    }

    public void ClosePanel()
    {
        if (_openedPanel.Count <= 0)
        {
            OpenPausePanel();
        }
        else
        {
            _openedPanel.Pop().SetActive(false);
        }
    }

    private void OpenPausePanel()
    {
        _pausePanel.SetActive(true);
        GameEventsManager.Instance.UIEvents.TogglePauseMenu(true);
        GameEventsManager.Instance.InputEvents.SetUI();
        AddOpenedPanel(_pausePanel);
        
    }
    
    public void ClosePausePanel()
    {
        ClosePanel();
        GameEventsManager.Instance.UIEvents.TogglePauseMenu(false);
        GameEventsManager.Instance.InputEvents.SetGame();
    }

    public void OnQuitButton()
    {
        GameEventsManager.Instance.UIEvents.QuitButtonPressed();
    }
}
