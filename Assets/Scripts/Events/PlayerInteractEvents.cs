
using System;

public class PlayerInteractEvents
{
    public event Action<TestWasteType> OnWastePickUpEvent;

    public void WastePickUp(TestWasteType wasteType)
    {
        OnWastePickUpEvent?.Invoke(wasteType);
    }
}

public enum TestWasteType
{
    Any,
    Plastic,
    Metal
}
