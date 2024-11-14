using System;

public class BatteryEvents
{
    public event Action<float> OnUpdateBatteryLifeEvent;
    public event Action<ChargingStation> OnInitializeChargingStationEvent;

    public void UpdateBatteryLife(float percent)
    {
        OnUpdateBatteryLifeEvent?.Invoke(percent);
    }
    
    public void InitializeChargingStation(ChargingStation chargingStation)
    {
        OnInitializeChargingStationEvent?.Invoke(chargingStation);
    }
}