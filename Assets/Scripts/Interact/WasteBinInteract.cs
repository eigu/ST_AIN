using System.Collections;
using System.Collections.Generic;
using Interact;
using UnityEngine;

public class WasteBinInteract : Interactable
{
    public InventoryInfoSO inventoryInfoSO;
    public WasteClassifications validWasteClassification;

    public override void OnPrimaryInteract()
    {
        GameEventsManager.Instance.HotbarEvents.PutCurrentItemInWasteBin(this);
    }

    public override void OnSecondaryInteract()
    {
        OpenInventory();
    }

    public override void OnFocus()
    {
        GameEventsManager.Instance.UIEvents.UpdateUIGuideText(guideText);
    }

    public override void OnLoseFocus()
    {
        GameEventsManager.Instance.UIEvents.UpdateUIGuideText("");
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
        GameEventsManager.Instance.InputEvents.SetUI();
    }
}
