using System;

public class QuestEvents
{
    public event Action<string> OnStartQuestEvent;
    public event Action<string> OnAdvanceQuestEvent;
    public event Action<string> OnFinishQuestEvent;
    public event Action<Quest> OnQuestStateChangeEvent;

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
}