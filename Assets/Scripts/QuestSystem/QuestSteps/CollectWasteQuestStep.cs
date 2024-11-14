using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will be put on a prefab so that it can track the step
public class CollectWasteQuestStep : QuestStep
{
    [SerializeField] private WasteClassifications _type;
    [SerializeField] private int _wasteToCollect = 5;
    private int _wasteCollected = 0;

    private void OnEnable()
    {
        GameEventsManager.Instance.PlayerInteractEvents.OnWastePickUpEvent += WasteCollected;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.PlayerInteractEvents.OnWastePickUpEvent -= WasteCollected;
    }

    private void WasteCollected(WasteInfoSO info, GameObject o)
    {
        if (_wasteCollected < _wasteToCollect)
        {
            if (_type == WasteClassifications.Any)
            {
                _wasteCollected++;
            }
            else
            {
                if (info.wasteClassification == _type)
                {
                    _wasteCollected++;
                }
            }
        }

        if (_wasteCollected >= _wasteToCollect)
        {
            FinishQuestStep();
        }
        
        UpdateState();
    }

    protected override void UpdateState()
    {
        string state = _wasteCollected.ToString();
        string stateDisplayText = $"Collect {_type} waste: {_wasteCollected}/{_wasteToCollect}";
        ChangeState(state,stateDisplayText);
    }

    protected override void SetQuestStepState(string state)
    {
        _wasteCollected = Int32.Parse(state);
        UpdateState();
    }
}
