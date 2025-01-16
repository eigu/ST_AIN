using System;
using UnityEngine;

public class UIPanelInventory : UIPanelBase
{
    public override void OnOpenPanel()
    {
        Debug.Log("Inventory Opened!");
    }

    public override void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}