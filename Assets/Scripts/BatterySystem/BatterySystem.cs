
using ScriptableObjectArchitecture;
using UnityEngine;

public class BatterySystem : MonoBehaviour
{
    [SerializeField] private FloatVariable _playerSpeed; 
    [SerializeField] private float _batteryTimeAllowance; 
    [SerializeField] private float _maxBatteryLife = 600f; 
    private float _currentBatteryLife;
    
    private float _updateInterval = 1f; // Update every second
    private float _timer;

    private ChargingStationManager _chargingStationManager;
    private bool _hasWarnedLowBattery;

    private void Awake()
    {
        _chargingStationManager = FindObjectOfType<ChargingStationManager>();

        if (_chargingStationManager == null) Debug.LogError("No Charging Station Manager Found.");
    }

    void Start()
    {
        ReplenishBattery();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _updateInterval)
        {
            ConsumeBattery(1f);
            _timer = 0f;
        }
    }
    
    private void ConsumeBattery(float amount)
    {
        _currentBatteryLife -= amount;

        if (_currentBatteryLife < 0)
        {
            _currentBatteryLife = 0;
            OnBatteryDepleted();
        }

        if(!_hasWarnedLowBattery) CheckChargingStation();
        UpdateBatteryUI();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckChargingStation()
    {
        if ((_currentBatteryLife - _batteryTimeAllowance) <= _chargingStationManager.GetTimeBeforeReachingNearestStation(transform, _playerSpeed.Value))
        {
            Debug.Log("Low Battery!!! Go to the nearest charging station!");
            _hasWarnedLowBattery = true;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnBatteryDepleted()
    {
        Debug.Log("Battery Low, Saint 1 Shutting down...");
    }


    public void ReplenishBattery()
    {
        _currentBatteryLife = _maxBatteryLife;
        _hasWarnedLowBattery = false;
        UpdateBatteryUI(); 
    }
    
    private void UpdateBatteryUI()
    {
        float batteryPercentage = (_currentBatteryLife / _maxBatteryLife) * 100f;
       GameEventsManager.Instance.BatteryEvents.UpdateBatteryLife(batteryPercentage);
    }
    
    //TODO: Decrease Battery Capacity
}
