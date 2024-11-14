using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargingStationManager : MonoBehaviour
{
    public LayerMask _playerLayer;
    private List<ChargingStation> _chargingStations = new List<ChargingStation>();
    
    private void OnEnable()
    {
        GameEventsManager.Instance.BatteryEvents.OnInitializeChargingStationEvent += AddChargingStation;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.BatteryEvents.OnInitializeChargingStationEvent -= AddChargingStation;
    }

    private void AddChargingStation(ChargingStation ChargingStation)
    {
        _chargingStations.Add(ChargingStation);
    }
    
    private ChargingStation GetNearestChargingStationToPlayer(Transform player)
    {
        if (_chargingStations.Count == 0)
        {
            Debug.LogError("No Charging Stations Found.");
            return null; 
        }

        ChargingStation nearestStation = null;
        float nearestDistance = float.MaxValue;

        foreach (var station in _chargingStations)
        {
            float distance = Vector3.Distance(player.position, station.transform.position);
            
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestStation = station;
            }
        }

        return nearestStation; // Return the nearest charging station found
    }

    public float GetTimeBeforeReachingNearestStation(Transform player, float playerSpeed)
    {
        ChargingStation nearestStation = GetNearestChargingStationToPlayer(player);
    
        if (nearestStation == null || playerSpeed <= 0)
        {
            return float.MaxValue; // Return a large value if no station is found or speed is invalid
        }

        Transform target = nearestStation.transform;
        NavMeshPath path = new NavMeshPath();

        // Calculate the path from player to nearest charging station
        if (NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path))
        {
            float totalDistance = 0f;

            // Calculate total distance along the path
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            // Calculate time to reach the nearest station
            return totalDistance / playerSpeed; // Time = Distance / Speed
        }

        return float.MaxValue; // Return a large value if path calculation fails
    }
}
