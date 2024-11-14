using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject _inventoryContainer;
    
    [Header("Inventory Slots")]
    [SerializeField] private TextMeshProUGUI _inventoryNameTMP;
    [SerializeField] private RectTransform _slotsContainer;

    [Header("Inventory Description")] 
    [SerializeField] private Image _selectedIcon;
    [SerializeField] private TextMeshProUGUI _selectedNameTMP;
    [SerializeField] private TextMeshProUGUI _selectedClassificationTMP;
    [SerializeField] private TextMeshProUGUI _selectedDescriptionTMP;
    [SerializeField] private GameObject _inventoryDescriptionContainer;
    [SerializeField] private GameObject _noSelectedIndicator;
    //[SerializeField] private TextMeshProUGUI _inventoryNameTMP;
    
    private List<InventorySlotUI> _slots = new List<InventorySlotUI>();
    private InventoryInfoSO _previousInventory;
    private int _lastSelectedIndex;

    private void Awake()
    {
        foreach (RectTransform rectTransform in _slotsContainer)
        {
            _slots.Add(rectTransform.GetComponent<InventorySlotUI>());
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.InventoryEvents.OnOpenInventoryEvent += OpenInventory;
        GameEventsManager.Instance.InventoryEvents.OnSelectInventorySlotUIEvent += SetUpDescriptionSection;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnOpenInventoryEvent -= OpenInventory;
        GameEventsManager.Instance.InventoryEvents.OnSelectInventorySlotUIEvent -= SetUpDescriptionSection;
    }

    private void OpenInventory(InventoryInfoSO inventoryInfo)
    {
        _inventoryContainer.SetActive(true);
        
        if (inventoryInfo != _previousInventory) _lastSelectedIndex = 0;

        DeactivateActiveSlots();

        for (int i = 0; i < inventoryInfo.maxInventoryCapacity; i++)
        {
            _slots[i].gameObject.SetActive(true);
            _slots[i].ResetSlot();
        }
        
        _inventoryNameTMP.text = inventoryInfo.inventoryName;

        if (inventoryInfo.inventory.Count <= 0)
        {
            ToggleInventoryDescription(false);
            return;
        }
        
        ToggleInventoryDescription(true);
        
        int index = 0;
        
        foreach (var kvp in inventoryInfo.inventory)
        {
            if (kvp.Value.quantity > 1)
            {
                for (int i = 0; i < kvp.Value.quantity; i++)
                {
                    SetUpIconButton(index, kvp.Value.wasteInfo );
                    index++;
                }
            }
            else if(kvp.Value.quantity == 1)
            {
                SetUpIconButton(index, kvp.Value.wasteInfo);
                index++;
            }
           
        }
        
        _slots[_lastSelectedIndex].SelectSlot(); 
    }

    private void DeactivateActiveSlots()
    {
        foreach (var slot in _slots)
        {
            if (slot.gameObject.activeInHierarchy)
            {
                slot.gameObject.SetActive(false);
            }
            else
            {
                break;
            }
        }
    }

    private void SetUpIconButton(int index, WasteInfoSO wasteInfo)
    {
        _slots[index].SetWasteInfo(wasteInfo);
    }
    
    private void SetUpDescriptionSection(WasteInfoSO wasteInfoSo)
    {
        if (wasteInfoSo)
        {
            ToggleInventoryDescription(true);
            _selectedIcon.sprite = wasteInfoSo.icon;
            _selectedNameTMP.text = wasteInfoSo.displayName;
            _selectedClassificationTMP.text = GetStringOfClassification(wasteInfoSo.wasteClassification);
            _selectedDescriptionTMP.text =
                string.IsNullOrWhiteSpace(wasteInfoSo.description) ? "No waste description available." : wasteInfoSo.description;
        }
        else
        {
            ToggleInventoryDescription(false);
        }
    }

    private string GetStringOfClassification(WasteClassifications wasteClassification)
    {
        string classificationString = "";
        
        switch (wasteClassification)
        {
            case WasteClassifications.Any:
                classificationString = "";
                break;
            case WasteClassifications.Biodegradable:
                classificationString = "Biodegradable Waste";
                break;
            case WasteClassifications.Recyclable:
                classificationString = "Recyclable Waste";
                break;
            case WasteClassifications.Residual:
                classificationString = "Residual Waste";
                break;
            case WasteClassifications.Special:
                classificationString = "Special Waste";
                break;
        }

        return classificationString;
    }

    public void CloseInventory()
    {
        _inventoryContainer.SetActive(false);
    }

    private void ToggleInventoryDescription(bool show)
    {
        _inventoryDescriptionContainer.SetActive(show);
        _noSelectedIndicator.SetActive(!show);
    }

    
    
}
