using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _batteryPercent;
    [SerializeField] private Image _batteryImage;
    [SerializeField] private bool _wholeNumber;

    private void OnEnable()
    {
        GameEventsManager.Instance.BatteryEvents.OnUpdateBatteryLifeEvent += UpdateBatteryUI;
    }
    
    private void OnDestroy()
    {
        GameEventsManager.Instance.BatteryEvents.OnUpdateBatteryLifeEvent -= UpdateBatteryUI;
    }

    private void UpdateBatteryUI(float percent)
    {
        _batteryImage.fillAmount = percent / 100;
        
        _batteryPercent.text = _wholeNumber ? $"{percent:F0}%" : $"{percent:F1}%";
    }
}