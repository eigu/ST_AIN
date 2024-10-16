using System;
using UnityEngine;

public class TrashBinFollow : MonoBehaviour
{
    public Transform trashBinTarget;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public float minDistanceToTarget = 2f;
    public float decelerationTime = 1f;

    private float currentSpeed;

    private void Start()
    {
        currentSpeed = followSpeed;
    }

    private void Update()
    {
        if (trashBinTarget == null) return;

        // get distance to target
        float distanceToTargetBin = Vector3.Distance(transform.position, trashBinTarget.position);
        Vector3 directionToTarget = (trashBinTarget.position - transform.position).normalized;

        if (distanceToTargetBin > minDistanceToTarget)
        {
            // rotate
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // move
            currentSpeed = Mathf.Lerp(currentSpeed, followSpeed, Time.deltaTime / decelerationTime);
            Vector3 movement = directionToTarget * (currentSpeed * Time.deltaTime);
            transform.Translate(movement, Space.World);
        }
        else
        {
            //stop
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / decelerationTime);
            Vector3 movement = directionToTarget * (currentSpeed * Time.deltaTime);
            transform.Translate(movement, Space.World);
        }
    }
}