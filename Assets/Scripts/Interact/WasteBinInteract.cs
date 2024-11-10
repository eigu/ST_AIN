using System.Collections;
using System.Collections.Generic;
using Interact;
using UnityEngine;

public class WasteBinInteract : Interactable
{
    public InventoryInfoSO inventoryInfoSO;
    public WasteClassifications validWasteClassification;

    public override void OnInteract()
    {
        GameEventsManager.Instance.HotbarEvents.PutCurrentItemInWasteBin(this);
    }

    public override void OnFocus()
    {
        
    }

    public override void OnLoseFocus()
    {
        
    }

    public override void OnNear()
    {
        
    }

    public override void OnLoseNear()
    {
        
    }

    public void OpenInventory()
    {
        GameEventsManager.Instance.InventoryEvents.OpenInventory(inventoryInfoSO);
    }
}
