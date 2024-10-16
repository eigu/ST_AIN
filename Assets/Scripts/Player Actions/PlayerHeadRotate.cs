using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadRotate : MonoBehaviour
{
    
    [Header("Rig Look at")]
    [SerializeField] private Transform headLookAt;
    [SerializeField] private Transform head;
    [SerializeField] private float lookAtDistance;
    [SerializeField] private float lookSmooth;
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

        // Check if the angle is greater than 90 degrees
        if (angle < 90f)
        {
            headTargetPosition = head.position + lookDirection * lookAtDistance;
        }
        else
        {
            headTargetPosition = head.position + head.forward * lookAtDistance;
        }

        headLookAt.position = Vector3.Lerp(headLookAt.position, headTargetPosition, Time.deltaTime * lookSmooth);
    }
}
