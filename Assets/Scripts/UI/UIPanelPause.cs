using System;
using UnityEngine;

public class UIPanelPause : UIPanelBase
{
    public override void OnOpenPanel()
    {
        GameEventsManager.Instance.UIEvents.TogglePauseMenu(true);
    }

    public override void ClosePanel()
    {
        GameEventsManager.Instance.UIEvents.TogglePauseMenu(false);
        gameObject.SetActive(false);
    }
}