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
}
