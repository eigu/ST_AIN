using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DrawPathManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _pathRenderer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _pathHeightOffset = 1.25f;
    [SerializeField] private float _pathUpdateSpeed = 0.25f;
    
    private Coroutine _drawPathCoroutine;
    private Transform _target;
    private Transform _player;
    

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;

        if (_player == null)
        {
            Debug.LogWarning("Player not found in scene, path finding will not work.");
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.PathFindingEvents.OnStartTrackingTargetEvent += TrackTarget;
        GameEventsManager.Instance.PathFindingEvents.OnStopTrackingEvent += StopTrackingTarget;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.PathFindingEvents.OnStartTrackingTargetEvent -= TrackTarget;
        GameEventsManager.Instance.PathFindingEvents.OnStopTrackingEvent -= StopTrackingTarget;
    }

    private void TrackTarget(Transform target)
    {
        _target = target;
        _pathRenderer.gameObject.SetActive(true);
        
        if (_drawPathCoroutine != null)
        {
            StopCoroutine(_drawPathCoroutine);
        }
        
        _drawPathCoroutine = StartCoroutine(DrawPathToTarget());
    }

    private void StopTrackingTarget(Transform target)
    {
        if (target != _target) return;
        
        _target = null;
        _pathRenderer.gameObject.SetActive(false);
        
        if (_drawPathCoroutine != null)
        {
            StopCoroutine(_drawPathCoroutine);
        }
        
        
    }
    
    private IEnumerator DrawPathToTarget()
    {
        NavMeshPath path = new NavMeshPath();

        while (_target)
        {
            if (NavMesh.CalculatePath(_player.position, _target.position, NavMesh.AllAreas, path))
            {
                List<Vector3> points = new List<Vector3>();


                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Vector3 startPoint = path.corners[i];
                    Vector3 endPoint = path.corners[i + 1];
                    Vector3 direction = (endPoint - startPoint).normalized;

                    // Raycast along the direction to find points
                    float distance = Vector3.Distance(startPoint, endPoint);
                    float currentDistance = 0f;

                    while (currentDistance < distance)
                    {
                        Vector3 raycastPoint = startPoint + direction * currentDistance;

                        if (Physics.Raycast(raycastPoint + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5, ~_playerLayer))
                        {
                            points.Add(hit.point + Vector3.up * _pathHeightOffset);
                        }

                        currentDistance += 0.5f; 
                    }
                }

                // Update LineRenderer with calculated points
                _pathRenderer.positionCount = points.Count;
                for (int i = 0; i < points.Count; i++)
                {
                    _pathRenderer.SetPosition(i, points[i]);
                }
            }
            
            yield return new WaitForSeconds(_pathUpdateSpeed);
        }
    }
}
