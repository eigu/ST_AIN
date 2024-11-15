using System;
using System.Collections;
using System.Collections.Generic;
using Interact;
using UnityEngine;
using UnityEngine.Events;

public class NPCInteract : Interactable
{
    public QuestInfoSO questInfoSo;
    public bool _canTalk = true;
    [SerializeField] [Tooltip("NPC Identifier")]
    private string _identifier;
    [SerializeField] [Tooltip("Yarn Spinner node names.")]
    private List<string> _startNodeNames;
    [SerializeField] [Tooltip("Disable bark when dialogue is playing.")]
    private BarkUI _bark;
    private int _currentNodeIndex;
    
    public List<DialogueCommandHandler> commandHandlers = new List<DialogueCommandHandler>();
    
    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnFindDestinationTargetEvent += SendDestinationTarget;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnFindDestinationTargetEvent -= SendDestinationTarget;
    }

    private void SendDestinationTarget(string destinationIdentifier)
    {
        if (destinationIdentifier.Equals(_identifier))
        {
            GameEventsManager.Instance.QuestEvents.SendDestinationTarget(_identifier, transform);
            GameEventsManager.Instance.PathFindingEvents.StartTrackingTarget(transform);
        }
        
    }
    
    public void StartDialogue()
    {
        if (!_canTalk) return;
        GameEventsManager.Instance.QuestEvents.TalkToNPC(_identifier);
        GameEventsManager.Instance.PathFindingEvents.StopTracking(transform);
        
        foreach (var commandHandler in commandHandlers)
        {
            GameEventsManager.Instance.DialogueEvents.AddDialogueCommand(commandHandler);
            //make this one by one so that YarnSpinner recognize the commandName being mentioned
        }
        
        if (_startNodeNames.Count > 0 && _currentNodeIndex >= 0 && _currentNodeIndex < _startNodeNames.Count)
        {
            GameEventsManager.Instance.DialogueEvents.StartDialogue(_startNodeNames[_currentNodeIndex]);
        }
        else
        {
            return;
        }
        
        if (_bark) _bark.StopBark();
            
    }
    

    public void IncreaseDialogueIndex()
    {
        _currentNodeIndex++;
    }

    public void EndDialogue()
    {
        if (_bark) _bark.StartShowingBark();
        
        GameEventsManager.Instance.InputEvents.SetGame();
        
        foreach (var commandHandler in commandHandlers)
        {
            GameEventsManager.Instance.DialogueEvents.RemoveDialogueCommand(commandHandler);
        }
        
        
    }
    
    public void StartQuest()
    {
        GameEventsManager.Instance.QuestEvents.StartQuest(questInfoSo.ID);
        GameEventsManager.Instance.QuestEvents.OnFinishQuestStepEvent += OnFinishQuestCollect;
    }

    private void OnFinishQuestCollect(string id)
    {
        IncreaseDialogueIndex();
        Debug.Log("Step Done");
        
        GameEventsManager.Instance.QuestEvents.OnFinishQuestStepEvent -= OnFinishQuestCollect;
    }

    public void FinishNPC()
    {
        _canTalk = false;
    }


    public override void OnPrimaryInteract()
    {
        StartDialogue();
    }

    public override void OnSecondaryInteract()
    {
    }

    public override void OnFocus()
    {
        GameEventsManager.Instance.UIEvents.UpdateUIGuideText(guideText);
    }

    public override void OnLoseFocus()
    {
        GameEventsManager.Instance.UIEvents.UpdateUIGuideText("");
    }

    public override void OnNear()
    {
    }

    public override void OnLoseNear()
    {
    }
}

[Serializable]
public struct DialogueCommandHandler
{
    public string commandName;
    public UnityEvent del;
}