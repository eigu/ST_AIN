using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will be put on a prefab so that it can track the step
public class ReachDestinationQuestStep : QuestStep
{
    [SerializeField] private string _targetDestinationIdentifier;
    [SerializeField] private string _targetDestinationDisplayName;
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
        
        GameEventsManager.Instance.QuestEvents.FindDestinationTarget(_targetDestinationIdentifier);
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnReachDestinationEvent -= ReachDestination;
        GameEventsManager.Instance.QuestEvents.OnSendDestinationTargetEvent -= LocateTarget;
    }

    private void Update()
    {
        if (_isLocated )
        {
            _distance = (int)Vector3.Distance(_player.position, _target.position);
            UpdateState();
        }
    }

    private void LocateTarget(string destinationIdentifier, Transform target)
    {
        Debug.Log("ASD");
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
        string stateDisplayText = $"Go To {_targetDestinationDisplayName}: {_distance}m";
        ChangeState(state,stateDisplayText);
    }

    protected override void SetQuestStepState(string state)
    {
        _distance = Int32.Parse(state);
        UpdateState();
    }
}