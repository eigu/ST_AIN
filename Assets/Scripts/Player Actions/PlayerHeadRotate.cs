using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHeadRotate : MonoBehaviour
{
    
    [Header("Rig Look at")]
    [SerializeField] private Transform headLookAt;
    [SerializeField] private Transform head;
    [SerializeField] private float lookAtDistance;
    [SerializeField] private float lookSmooth;
    [SerializeField] private MultiAimConstraint _multiAimConstraint;
    private Vector3 headTargetPosition;
   

    public PlayerLook playerLook;

    void LateUpdate()
    {
        HeadRotation();
    }
    
    private void HeadRotation()
    {
        Vector3 lookDirection = playerLook.CameraTargetObject.transform.rotation * Vector3.forward;
        float angle = Vector3.Angle(head.forward, lookDirection);
        headTargetPosition = head.position + lookDirection * lookAtDistance;
        headLookAt.position = Vector3.Lerp(headLookAt.position, headTargetPosition, Time.deltaTime * lookSmooth);

        // Check if the angle is greater than 90 degrees
        if (angle < 90f)
        {
            _multiAimConstraint.weight = Mathf.Lerp(_multiAimConstraint.weight, 1f, Time.deltaTime * lookSmooth);
        }
        else
        {
            _multiAimConstraint.weight = Mathf.Lerp(_multiAimConstraint.weight, 0f, Time.deltaTime * lookSmooth);
        }

        
    }
}
