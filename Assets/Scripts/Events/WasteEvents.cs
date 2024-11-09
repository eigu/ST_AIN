using System;
using System.Collections.Generic;
using UnityEngine;

public class WasteEvents
{
    public event Action<string> OnValidateWasteEvent;
    public event Action<string, Vector3, Quaternion> OnSpawnWasteGameObjectEvent;

    public void ValidateWaste(string id)
    {
        OnValidateWasteEvent?.Invoke(id);
    }

    public void SpawnWasteGameObject(string id, Vector3 pos, Quaternion rot)
    {
        OnSpawnWasteGameObjectEvent?.Invoke(id, pos, rot);
    }
}