using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestManager : MonoBehaviour
{
    [Tooltip("Main Quests in order.")]
    public List<MainQuest> mainQuests = new List<MainQuest>();
    private int _currentMainQuestIndex = -1;
    
    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent += FinishQuest;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnFinishQuestEvent -= FinishQuest;
    }

    private IEnumerator Start()
    {

        return StartNextMainQuest();
            
    }

    IEnumerator StartNextMainQuest()
    {
        _currentMainQuestIndex++;

        if (_currentMainQuestIndex >= mainQuests.Count) yield break;

        if (!mainQuests[_currentMainQuestIndex].isAutomaticNextQuest) yield break;

        yield return new WaitForSeconds(mainQuests[_currentMainQuestIndex].waitTimeToNextQuest);
            
        GameEventsManager.Instance.QuestEvents.StartQuest(mainQuests[_currentMainQuestIndex].info.ID);
    } 


    private void FinishQuest(string questID)
    {
        if (_currentMainQuestIndex >= mainQuests.Count) return;
        if (!mainQuests[_currentMainQuestIndex].info.ID.Equals(questID)) return;
        StartCoroutine(StartNextMainQuest());
    }
    
    
}

[Serializable]
public struct MainQuest
{
    [Tooltip("Quests serializable object.")]
    public QuestInfoSO info;
    [Tooltip("Will automatically proceed to next step.")]
    public bool isAutomaticNextQuest;
    [Tooltip("Time it will take before triggering the next step (if isAutomaticNextQuest is true).")]
    public float waitTimeToNextQuest;
}
