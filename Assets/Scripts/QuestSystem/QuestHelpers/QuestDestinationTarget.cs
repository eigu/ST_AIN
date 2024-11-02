using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDestinationTarget : MonoBehaviour
{
    [SerializeField] private string _destinationIdentifier;

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
        if (destinationIdentifier.Equals(_destinationIdentifier))
        {
            GameEventsManager.Instance.QuestEvents.SendDestinationTarget(_destinationIdentifier, transform);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            GameEventsManager.Instance.QuestEvents.ReachDestination(_destinationIdentifier);
        }
    }
}
