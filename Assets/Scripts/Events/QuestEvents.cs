using System;

public class QuestEvents
{
    public event Action<string> OnStartQuestEvent;
    public event Action<string> OnAdvanceQuestEvent;
    public event Action<string> OnFinishQuestEvent;
    public event Action<Quest> OnQuestStateChangeEvent;
    
    public event Action<string, int, bool, QuestStepData> OnQuestStepDataChangeEvent;
    
    public event Action<string, string, bool, int, QuestType> OnStartQuestUIEvent;
    public event Action<string, int, string, bool> OnQuestStepDataChangeUIEvent;

    public void StartQuest(string id)
    {
        OnStartQuestEvent?.Invoke(id);
    }
    
    public void AdvanceQuest(string id)
    {
        OnAdvanceQuestEvent?.Invoke(id);
    }
    
    public void FinishQuest(string id)
    {
        OnFinishQuestEvent?.Invoke(id);
    }
    
    public void QuestStateChange(Quest quest)
    {
        OnQuestStateChangeEvent?.Invoke(quest);
    }
    
    public void QuestStepDataChange(string id, int stepIndex, bool isFinished, QuestStepData questStepData)
    {
        OnQuestStepDataChangeEvent?.Invoke(id, stepIndex, isFinished, questStepData);
    }
    
    public void StartQuestUI(string id, string text, bool isSequential, int stepCount, QuestType type)
    {
        OnStartQuestUIEvent?.Invoke(id, text, isSequential, stepCount, type);
    }
    
    public void QuestStepDataChangeUI(string id, int stepIndex, string questStepTextDisplay, bool isFinished)
    {
        OnQuestStepDataChangeUIEvent?.Invoke(id, stepIndex, questStepTextDisplay, isFinished);
    }
    
    
    
}
