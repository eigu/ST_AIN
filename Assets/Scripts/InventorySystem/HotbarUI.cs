using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    [SerializeField] private GameObject _hotbarSlotPrefab;
    [SerializeField] private RectTransform _hotbarSlotContainer;
    private List<Button> _hotbarElements = new List<Button>();

    private void OnEnable()
    {
        GameEventsManager.Instance.HotbarEvents.OnSetUpHotbarUIEvent += SetUpHotbarUI;
        GameEventsManager.Instance.HotbarEvents.OnAddItemOnHotbarUIEvent += AddItemOnHotbarUI;
        GameEventsManager.Instance.HotbarEvents.OnRemoveItemOnHotbarUIEvent += RemoveItemOnHotbarUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.HotbarEvents.OnSetUpHotbarUIEvent -= SetUpHotbarUI;
        GameEventsManager.Instance.HotbarEvents.OnAddItemOnHotbarUIEvent -= AddItemOnHotbarUI;
        GameEventsManager.Instance.HotbarEvents.OnRemoveItemOnHotbarUIEvent -= RemoveItemOnHotbarUI;
    }

    private void SetUpHotbarUI(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Button b = Instantiate(_hotbarSlotPrefab, _hotbarSlotContainer).GetComponent<Button>();
            if(i == 0) b.transform.GetChild(0).gameObject.SetActive(true);
            _hotbarElements.Add(b);
            b.image.sprite = null; 
            b.image.enabled = false;
        }
    }

    private void AddItemOnHotbarUI(Sprite wasteSprite)
    {
        if (wasteSprite == null)
        {
            Debug.LogWarning("Waste sprite is null. Cannot update hotbar UI.");
            return;
        }

        if (_hotbarElements == null || _hotbarElements.Count == 0)
        {
            Debug.LogWarning("Hotbar elements list is empty or null. Cannot update hotbar UI.");
            return;
        }

        // Move sprites downwards
        for (int i = _hotbarElements.Count - 1; i > 0; i--)
        {
            if (_hotbarElements[i] != null && _hotbarElements[i - 1] != null)
            {
                _hotbarElements[i].image.sprite = _hotbarElements[i - 1].image.sprite;

                // Enable the image component only if the new sprite is not null
                if (_hotbarElements[i].image.sprite != null) _hotbarElements[i].image.enabled = true;
            }
            else
            {
                Debug.LogWarning($"Hotbar element at index {i} or {i - 1} is null. Skipping this update.");
            }
        }

        if (_hotbarElements[0] != null)
        {
            _hotbarElements[0].image.sprite = wasteSprite;
            _hotbarElements[0].image.enabled = true; // Ensure the image is enabled
        }
        else
        {
            Debug.LogWarning("Hotbar element at index 0 is null. Cannot set waste sprite.");
        }
    }

    private void RemoveItemOnHotbarUI()
    {
        // Check if there are any hotbar elements
        if (_hotbarElements == null || _hotbarElements.Count == 0)
        {
            Debug.LogWarning("Hotbar elements list is empty or null. Cannot remove from hotbar UI.");
            return;
        }

        // Move sprites upwards
        for (int i = 0; i < _hotbarElements.Count - 1; i++)
        {
            // make sure the current button and next button are valid before accessing their images
            if (_hotbarElements[i] != null && _hotbarElements[i + 1] != null)
            {
                _hotbarElements[i].image.sprite = _hotbarElements[i + 1].image.sprite;

                // Enable the image component only if the new sprite is not null
                _hotbarElements[i].image.enabled = _hotbarElements[i].image.sprite != null;
            }
            else
            {
                Debug.LogWarning($"Hotbar element at index {i} or {i + 1} is null. Skipping this update.");
            }
        }

        int lastIndex = _hotbarElements.Count - 1;
        
        if (_hotbarElements[lastIndex] != null)
        {
            _hotbarElements[lastIndex].image.sprite = null; 
            _hotbarElements[lastIndex].image.enabled = false;
        }
    }
}
