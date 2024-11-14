using UnityEngine;

public class TrackObject : MonoBehaviour
{
    public void StartTrack()
    {
        GameEventsManager.Instance.PathFindingEvents.StartTrackingTarget(transform);
    }

    public void StopTrack()
    {
        GameEventsManager.Instance.PathFindingEvents.StopTracking(transform);
    }
}