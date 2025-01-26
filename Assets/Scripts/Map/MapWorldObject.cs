using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWorldObject : MonoBehaviour
{
    [SerializeField] private MapIconDataSO mapIconInfo;
    [SerializeField] private string overrideName = "";
    [SerializeField] private float overrideScaleMultiplier = 1;
    [SerializeField] private bool isHighlight;
    

    private void Start()
    {
        GameEventsManager.Instance.UIEvents.RegisterMapWorldObjectIcon(transform, mapIconInfo, overrideName, isHighlight, overrideScaleMultiplier);
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.UIEvents.UnregisterMapWorldObjectIcon(transform);
    }
}