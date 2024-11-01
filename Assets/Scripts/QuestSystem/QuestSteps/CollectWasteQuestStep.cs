using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will be put on a prefab so that it can track the step
public class CollectWasteQuestStep : QuestStep
{
    [SerializeField] private TestWasteType _type;
    [SerializeField] private int _wasteToCollect = 5;
    private int _wasteCollected = 0;

    private void OnEnable()
    {
        //subscribe to collect waste with parameter of waste type
    }
    
    private void OnDisable()
    {
        //unsubscribe
    }

    private void WasteCollected(TestWasteType type)
    {
        if (_wasteCollected < _wasteToCollect)
        {
            if (_type == TestWasteType.Any)
            {
                _wasteCollected++;
                UpdateState();
            }
            else
            {
                if (type == _type)
                {
                    _wasteCollected++;
                    UpdateState();
                }
            }
        }

        if (_wasteCollected >= _wasteToCollect)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = _wasteCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _wasteCollected = Int32.Parse(state);
        UpdateState();
    }
}

public enum TestWasteType
{
    Any,
    Plastic,
    Metal
}
