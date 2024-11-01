using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class QuestStep : MonoBehaviour
{
    private bool _isFinished = false;

    private string _questID;
    private int _stepIndex;

    public void InitializeQuestStep(string questID, int stepIndex, string questStepState)
    {
        _questID = questID;
        _stepIndex = stepIndex;
        if (!string.IsNullOrEmpty(questStepState))
        {
            SetQuestStepState(questStepState);
        }
    }
    
    protected void FinishQuestStep()
    {
        if (!_isFinished)
        {
            _isFinished = true;
            GameEventsManager.Instance.QuestEvents.AdvanceQuest(_questID);
            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState)
    {
        GameEventsManager.Instance.QuestEvents.QuestStepDataChange(_questID, _stepIndex, new QuestStepData(newState));
    }

    protected abstract void SetQuestStepState(string state);
}
