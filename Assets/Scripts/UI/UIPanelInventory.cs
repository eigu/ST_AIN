using System;
using UnityEngine;

public class UIPanelInventory : UIPanelBase
{
    public override void OnOpenPanel()
    {
        
    }

    public override void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}