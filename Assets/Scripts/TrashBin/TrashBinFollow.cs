using System;
using UnityEngine;

public class TrashBinFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public float minDistanceToTarget = 2f;
    public float decelerationTime = 1f;

    public Transform body;
    public float maxTiltAngle = 15f;
    private float bodyRestingZPos;
    public float bodyMaxZPos = 0.5f;
    private float currentSpeed;

    private bool _isFollowingTarget;
    private CharacterController _controller;
    private Vector3 _velocity;
    private readonly float _gravity = -9.81f;
    private bool _isGrounded;
    private float _verticalVelocity;

    private void Start()
    {
        currentSpeed = followSpeed;
        bodyRestingZPos = body.localPosition.z;
        _controller = GetComponent<CharacterController>();
        
        // Add CharacterController if it doesn't exist
        if (_controller == null)
        {
            _controller = gameObject.AddComponent<CharacterController>();
        }
    }

    private void Update()
    {
        if (target == null) return;

        HandleGroundCheck();
        HandleMovement();
        HandleRotation();
        HandleBodyAnimation();
    }

    private void HandleGroundCheck()
    {
        // Check if grounded
        _isGrounded = _controller.isGrounded;
        
        // Apply gravity
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f; // Small negative value to keep grounded
        }
        
        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void HandleMovement()
    {
        float distanceToTargetBin = Vector3.Distance(transform.position, target.position);

        if (distanceToTargetBin > minDistanceToTarget)
        {
            _isFollowingTarget = true;
            currentSpeed = Mathf.Lerp(currentSpeed, followSpeed, Time.deltaTime / decelerationTime);
        }
        else
        {
            _isFollowingTarget = false;
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / decelerationTime);
        }

        _velocity = transform.forward * currentSpeed;
        
        Vector3 movement = _velocity * Time.deltaTime;
        movement.y = _verticalVelocity * Time.deltaTime;

        _controller.Move(movement);
    }

    private void HandleRotation()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0; 
        
        if (directionToTarget != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
    private void HandleBodyAnimation()
    {
        // Tilt animation
        float tiltAngle = Mathf.Lerp(0, maxTiltAngle, (currentSpeed / followSpeed));
        Vector3 currentRotation = body.localEulerAngles;
        body.localEulerAngles = new Vector3(tiltAngle, currentRotation.y, currentRotation.z);
        
        // Height animation
        float targetHeight = Mathf.Lerp(bodyRestingZPos, bodyMaxZPos, currentSpeed / followSpeed);
        Vector3 currentBodyPos = body.localPosition;
        body.localPosition = new Vector3(currentBodyPos.x, currentBodyPos.y, targetHeight);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _isFollowingTarget ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceToTarget);
    }
}