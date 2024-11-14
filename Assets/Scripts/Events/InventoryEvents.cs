using System;

public class InventoryEvents
{
    public event Action<InventoryInfoSO> OnOpenInventoryEvent;
    public event Action<WasteInfoSO> OnSelectInventorySlotUIEvent;

    public void OpenInventory(InventoryInfoSO inventory)
    {
        OnOpenInventoryEvent?.Invoke(inventory);
    }

    public void SelectInventorySlotUI(WasteInfoSO wasteInfoSO)
    {
        OnSelectInventorySlotUIEvent?.Invoke(wasteInfoSO);
    }
}