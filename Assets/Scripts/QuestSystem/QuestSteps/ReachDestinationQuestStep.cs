using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will be put on a prefab so that it can track the step
public class ReachDestinationQuestStep : QuestStep
{
    [SerializeField] [Tooltip("Unique ID string used to find and identify target destination, should match with the target's QuestDestinationTarget's identifier.")]
    private string _targetDestinationIdentifier;
    
    [SerializeField] [Tooltip("Will be displayed on a UI Text.")]
    private string _targetDestinationDisplayName;
    
    [SerializeField] [Tooltip("Tracks the target destination in real time.")]
    private bool _shouldTrack;
    
    private Transform _target;
    private int _distance;
    private bool _isLocated;
    private Transform _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;

        if (_player == null) Debug.LogError("Player Not Found");
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnReachDestinationEvent += ReachDestination;
        GameEventsManager.Instance.QuestEvents.OnSendDestinationTargetEvent += LocateTarget;
        
        if (_shouldTrack) GameEventsManager.Instance.QuestEvents.FindDestinationTarget(_targetDestinationIdentifier);
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnReachDestinationEvent -= ReachDestination;
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
        if (_targetDestinationIdentifier.Equals(destinationIdentifier))
        {
            _target = target;
            _isLocated = true;
        }
    }

    private void ReachDestination(string destinationIdentifier)
    {
        if (_targetDestinationIdentifier.Equals(destinationIdentifier))
        {
            FinishQuestStep();
            UpdateState();
        }
        
        
    }
    
    protected override void UpdateState()
    {
        string state = _distance.ToString();
        string stateDisplayText = _shouldTrack ? $"Go To {_targetDestinationDisplayName}: {_distance}m." : $"Go To {_targetDestinationDisplayName}.";
        ChangeState(state,stateDisplayText);
    }

    protected override void SetQuestStepState(string state)
    {
        _distance = Int32.Parse(state);
        UpdateState();
    }
}