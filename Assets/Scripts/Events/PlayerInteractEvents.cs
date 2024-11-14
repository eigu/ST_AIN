
using System;
using UnityEngine;

public class PlayerInteractEvents
{
    public event Action<WasteInfoSO, GameObject> OnWastePickUpEvent;

    public void WastePickUp(WasteInfoSO waste, GameObject wasteGameObject)
    {
        OnWastePickUpEvent?.Invoke(waste, wasteGameObject);
    }
}

