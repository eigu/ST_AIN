using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will be put on a prefab so that it can track the step
public class TalkToNPCQuestStep : QuestStep
{
    [SerializeField] [Tooltip("Unique ID string used to find and identify target NPC, should match with the NPC identifier.")]
    private string _targetNPCIdentifier;
    
    [SerializeField] [Tooltip("Will be displayed on a UI Text.")]
    private string _targetNPCDisplayName;
    
    [SerializeField] [Tooltip("Tracks the target destination in real time.")]
    private bool _shouldTrack;
    
    private Transform _target;
    private int _distance;
    private bool _isLocated;
    private Transform _player;

    private void OnEnable()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;

        if (_player == null) Debug.LogError("Player Not Found");
        
        GameEventsManager.Instance.QuestEvents.OnTalkToNPCEvent += TalkedToNPC;
        GameEventsManager.Instance.QuestEvents.OnSendDestinationTargetEvent += LocateTarget;
        
        if (_shouldTrack) GameEventsManager.Instance.QuestEvents.FindDestinationTarget(_targetNPCIdentifier);
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnTalkToNPCEvent -= TalkedToNPC;
        GameEventsManager.Instance.QuestEvents.OnSendDestinationTargetEvent -= LocateTarget;
    }
    
    

    private void Update()
    {
        if (!_shouldTrack) return;
        
        if (_isLocated )
        {
            _distance = (int)Vector3.Distance(_player.position, _target.position);
            UpdateState();
        }

    }

    private void LocateTarget(string destinationIdentifier, Transform target)
    {
        if (_targetNPCIdentifier.Equals(destinationIdentifier))
        {
            _target = target;
            _isLocated = true;
        }
    }

    private void TalkedToNPC(string destinationIdentifier)
    {
        if (_targetNPCIdentifier.Equals(destinationIdentifier))
        {
            FinishQuestStep();
            UpdateState();
        }
        
        
    }
    
    protected override void UpdateState()
    {
        string state = _distance.ToString();
        string stateDisplayText = _shouldTrack ? $"Talk to {_targetNPCDisplayName}: {_distance}m." : $"Talk to {_targetNPCDisplayName}.";
        ChangeState(state,stateDisplayText);
    }

    protected override void SetQuestStepState(string state)
    {
        _distance = Int32.Parse(state);
        UpdateState();
    }
}