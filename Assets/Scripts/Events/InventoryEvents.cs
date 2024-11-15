using System;

public class InventoryEvents
{
    public event Action<InventoryInfoSO> OnOpenInventoryEvent;
    public event Action<WasteInfoSO> OnSelectInventorySlotUIEvent;
    
    public event Action OnCloseInventoryEvent;

    public void OpenInventory(InventoryInfoSO inventory)
    {
        OnOpenInventoryEvent?.Invoke(inventory);
    }
    
    public void CloseInventory()
    {
        OnCloseInventoryEvent?.Invoke();
    }

    public void SelectInventorySlotUI(WasteInfoSO wasteInfoSO)
    {
        OnSelectInventorySlotUIEvent?.Invoke(wasteInfoSO);
    }
}