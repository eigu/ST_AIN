using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInventoryHotbar : MonoBehaviour
{
    [SerializeField] private int maxSlotCount = 5;
    private int _currentHoldCount = 0;
    private Stack<WasteInfoSO> _hotbarContents = new Stack<WasteInfoSO>();

    private void OnEnable()
    {
        GameEventsManager.Instance.PlayerInteractEvents.OnWastePickUpEvent += CheckPickUp;
        GameEventsManager.Instance.HotbarEvents.OnPutCurrentItemInWasteBinEvent += PutCurrentItemInWasteBin;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.PlayerInteractEvents.OnWastePickUpEvent -= CheckPickUp;
        GameEventsManager.Instance.HotbarEvents.OnPutCurrentItemInWasteBinEvent -= PutCurrentItemInWasteBin;
    }

    private void Start()
    {
        GameEventsManager.Instance.HotbarEvents.SetUpHotbarUI(maxSlotCount);
    }

    private void CheckPickUp(WasteInfoSO w, GameObject o)
    {
        if (_currentHoldCount >= maxSlotCount) return;

        _hotbarContents.Push(w);
        
        GameEventsManager.Instance.HotbarEvents.AddItemOnHotbarUI(w.icon);
        
        Destroy(o); // pooling

        _currentHoldCount++;
        
    }
    
    public void CheckDrop()
    {
        if (_currentHoldCount <= 0) return;
        
        TestDrop(_hotbarContents.Pop().ID);
        
        GameEventsManager.Instance.HotbarEvents.RemoveItemOnHotbarUI();
        
        _currentHoldCount--;
        if (_currentHoldCount <= 0) _currentHoldCount = 0;
        
    }
    
    public void TestDrop(string id)
    {
       GameEventsManager.Instance.WasteEvents.SpawnWasteGameObject(id, transform.position + transform.forward * 1.5f, Quaternion.identity);
    }
    
    private void PutCurrentItemInWasteBin(WasteBinInteract wasteBin)
    {
        if (_currentHoldCount <= 0) return;

        if (wasteBin.validWasteClassification == _hotbarContents.Peek().wasteClassification)
        {
            // ui correct bin
            wasteBin.inventoryInfoSO.Add(_hotbarContents.Pop(), 1); // or just id??

            GameEventsManager.Instance.HotbarEvents.RemoveItemOnHotbarUI();

            _currentHoldCount--;
            if (_currentHoldCount <= 0) _currentHoldCount = 0;
        }
        else
        {
            Debug.Log("WRONG BIN!");
            // ui wrong bin
        }
    }
}
