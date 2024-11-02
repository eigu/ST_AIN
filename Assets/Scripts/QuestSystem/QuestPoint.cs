using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")] 
    [SerializeField] private QuestInfoSO _questInfo;

    private string _questID;
    private QuestState _currentQuestState;

    private void Awake()
    {
        if (_questInfo)
        {
            _questID = _questInfo.ID;
        }
        else
        {
            Debug.LogError("No Quest SO assigned.");
        }
    }

    private void Start()
    {
        //remove
        GameEventsManager.Instance.QuestEvents.StartQuest(_questInfo.ID);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnQuestStateChangeEvent += QuestStateChange;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnQuestStateChangeEvent -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.ID.Equals(_questInfo.ID))
        {
            _currentQuestState = quest.state;
            
        }
    }
}
