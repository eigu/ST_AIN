using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;

    private int _currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        info = questInfo;
        state = QuestState.RequirementsNotMet;
        _currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (_currentQuestStepIndex < info.questStepsPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();

        if (questStepPrefab != null)
        {
            Object.Instantiate(questStepPrefab, parentTransform);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepsPrefabs[_currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning($"Step index out of bounds. Quest ID: {info.ID}, Step Index: {_currentQuestStepIndex}");
        }

        return questStepPrefab;
    }
}
