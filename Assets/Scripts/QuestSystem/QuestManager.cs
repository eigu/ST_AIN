using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> _questMap;

    private void Awake()
    {
        _questMap = CreateQuestMap();
    }

    private void Start()
    {
        foreach (Quest q in _questMap.Values)
        {
            //uncomment if load script is done
            //if (quest.state == QuestState.InProgress) quest.InstantiateCurrentQuestStep(transform);
            GameEventsManager.Instance.QuestEvents.QuestStateChange(q);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestEvent += StartQuest;
        GameEventsManager.Instance.QuestEvents.OnAdvanceQuestEvent += AdvanceQuest;
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent += FinishQuest;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeEvent += QuestStepDataChange;

        // declare player data and subscribe to events that change player data for checking of requirements
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuestEvent -= StartQuest;
        GameEventsManager.Instance.QuestEvents.OnAdvanceQuestEvent -= AdvanceQuest;
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent -= FinishQuest;
        GameEventsManager.Instance.QuestEvents.OnQuestStepDataChangeEvent -= QuestStepDataChange;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;
        GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool requirementsMet = true;
        
        //other conditions like player chapter etc

        foreach (var prerequisite in quest.info.questPrerequisites)
        {
            if (GetQuestByID(prerequisite.ID).state != QuestState.Finished)
            {
                requirementsMet = false;
                break;
            }
        }

        return requirementsMet;
    }

    //subscribe to important change events like OnChapterChangeEvent
    private void UpdateQuest()
    {
        foreach (Quest quest in _questMap.Values)
        {
            if (quest.state == QuestState.RequirementsNotMet && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.ID, QuestState.CanStart);
            }
        }
    }
    

    private void StartQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrentQuestStep(transform);
        ChangeQuestState(quest.info.ID, QuestState.InProgress);
    }
    
    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestByID(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(transform);
        }
        else
        {
            ChangeQuestState(quest.info.ID, QuestState.CanFinish);
        }
        
    }
    
    private void FinishQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
    }

    private void ClaimRewards(Quest quest)
    {
        //put events that increase player stats
        //GameEventsManager.Instance.MoneyEvents.MoneyGained(quest.info.moneyReward);
        //and more
    }

    private void QuestStepDataChange(string id, int stepIndex, QuestStepData questStepData)
    {
        Quest quest = GetQuestByID(id);
        quest.StoreQuestStepData(questStepData, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (var questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.ID))
            {
                Debug.LogWarning($"Duplicate ID found when creating quest map: {questInfo.ID}");
            }
            
            idToQuestMap.Add(questInfo.ID, new Quest(questInfo));
            //uncomment if load script is done
            //idToQuestMap.Add(questInfo.ID, LoadQuest(questInfo));
        }

        return idToQuestMap;
    }

    private Quest GetQuestByID(string id)
    {
        Quest quest = _questMap[id];
        if (quest == null)
        {
            Debug.LogError($"ID not found in the quest map: {id}");
        }

        return quest;
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in _questMap.Values)
        {
            QuestData questData = quest.GetQuestData();
            SaveQuest();
        }
    }
    
    private void SaveQuest()
    {
        
    }

    private Quest LoadQuest(QuestInfoSO questInfoSo)
    {
        Quest quest = null;

        //deserialize quest

        return quest;
    }
}
