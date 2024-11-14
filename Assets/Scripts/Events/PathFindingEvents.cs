using System;
using UnityEngine;

public class PathFindingEvents
{
    public event Action<Transform> OnStartTrackingTargetEvent;
    public event Action<Transform> OnStopTrackingEvent;

    public void StartTrackingTarget(Transform target)
    {
        OnStartTrackingTargetEvent?.Invoke(target);
    }
    
    public void StopTracking(Transform target)
    {
        OnStopTrackingEvent?.Invoke(target);
    }
}