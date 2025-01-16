using System;
using UnityEngine;

public class UIPanel : UIPanelBase
{
    public override void OnOpenPanel()
    {
    }

    public override void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}