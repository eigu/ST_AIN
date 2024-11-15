using System;
using UnityEngine;

public class QuestEvents
{
    public event Action<string> OnStartQuestEvent;
    public event Action<string> OnFinishQuestStepEvent;
    public event Action<string> OnFinishQuestEvent;
    public event Action<Quest> OnQuestStateChangeEvent;
    
    public event Action<string, int, bool, QuestStepData> OnQuestStepDataChangeEvent;
    
    public event Action<string, string, bool, int, QuestType> OnStartQuestUIEvent;
    public event Action<string, int, string, bool> OnQuestStepDataChangeUIEvent;
    
    public event Action<string> OnReachDestinationEvent;
    public event Action<string> OnTalkToNPCEvent;
    public event Action<string, Transform> OnSendDestinationTargetEvent;
    public event Action<string> OnFindDestinationTargetEvent;

    public void StartQuest(string id)
    {
        OnStartQuestEvent?.Invoke(id);
    }
    
    public void FinishQuestStep(string id)
    {
        OnFinishQuestStepEvent?.Invoke(id);
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
    
    public void ReachDestination(string destinationIdentifier)
    {
        OnReachDestinationEvent?.Invoke(destinationIdentifier);
    }
    
    public void TalkToNPC(string NPCIdentifier)
    {
        OnTalkToNPCEvent?.Invoke(NPCIdentifier);
    }
    
    public void SendDestinationTarget(string destinationIdentifier, Transform target)
    {
        OnSendDestinationTargetEvent?.Invoke(destinationIdentifier, target);
    }
    
    public void FindDestinationTarget(string destinationIdentifier)
    {
        OnFindDestinationTargetEvent?.Invoke(destinationIdentifier);
    }
    
    
    
    
}
