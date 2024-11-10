using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _activeIndicator;
    private WasteInfoSO _waste;

    private void OnEnable()
    {
        GameEventsManager.Instance.InventoryEvents.OnSelectInventorySlotUIEvent += Unselect;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnSelectInventorySlotUIEvent -= Unselect;
    }

    private void Unselect(WasteInfoSO obj)
    {
        _activeIndicator.SetActive(false);
    }

    public void SetWasteInfo(WasteInfoSO waste)
    {
        _waste = waste;
        _button.image.sprite = waste.icon;
    }

    public void SelectSlot()
    {
        GameEventsManager.Instance.InventoryEvents.SelectInventorySlotUI(_waste);
        _activeIndicator.SetActive(true);
    }
}
