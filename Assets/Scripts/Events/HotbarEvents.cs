using System;
using System.Collections.Generic;
using UnityEngine;

public class HotbarEvents
{
    public event Action<int> OnSetUpHotbarUIEvent;
    public event Action<Sprite> OnAddItemOnHotbarUIEvent;
    public event Action OnRemoveItemOnHotbarUIEvent;
    
    public event Action<WasteBinInteract> OnPutCurrentItemInWasteBinEvent;
    

    public void SetUpHotbarUI(int count)
    {
        OnSetUpHotbarUIEvent?.Invoke(count);
    }
    
    public void AddItemOnHotbarUI(Sprite wasteSprite)
    {
        OnAddItemOnHotbarUIEvent?.Invoke(wasteSprite);
    }
    
    public void RemoveItemOnHotbarUI()
    {
        OnRemoveItemOnHotbarUIEvent?.Invoke();
    }

    public void PutCurrentItemInWasteBin(WasteBinInteract wasteBinInteract)
    {
        OnPutCurrentItemInWasteBinEvent?.Invoke(wasteBinInteract);
    }
    
    
}