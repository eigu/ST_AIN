using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will be put on a prefab so that it can track the step
public class CollectWasteQuestStep : QuestStep
{
    private TestWasteType _type;
    private int _wasteToCollect = 5;
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
            }
            else
            {
                if (type == _type)
                {
                    _wasteCollected++;
                }
            }
        }

        if (_wasteCollected >= _wasteToCollect)
        {
            FinishQuestStep();
        }
    }
}

public enum TestWasteType
{
    Any,
    Plastic,
    Metal
}
