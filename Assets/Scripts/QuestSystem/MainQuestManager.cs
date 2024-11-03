using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestManager : MonoBehaviour
{
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
        StartCoroutine(StartNextMainQuest());
    }
    
    
}

[Serializable]
public struct MainQuest
{
    public QuestInfoSO info;
    public bool isAutomaticNextQuest;
    public float waitTimeToNextQuest;
}
