using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    private PlayerAnimationManager _am;
    
    [Header("Player Grounded")]
    [SerializeField] private bool Grounded = true;
    [SerializeField] private float GroundedOffset = -0.14f; //Useful for rough ground
    [SerializeField] private float GroundedRadius = 0.5f; //Should match the radius of the CharacterController
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private Vector3 groundSpherePosition;
    
    public bool GetIsGrounded => Grounded;

    private void Awake()
    {
        _am = GetComponent<PlayerAnimationManager>();
        GroundedRadius = GetComponent<CharacterController>().radius;
    }

    private void Update()
    {
        GroundedCheck();
    }
    
    private void GroundedCheck()
    {
        // set sphere position, with offset
        groundSpherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(groundSpherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			
        // update animator if using character
        if (_am.HasAnimator)
        {
            _am.PlayerAnim.SetBool(_am.AnimIDIsGrounded, Grounded);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(groundSpherePosition, GroundedRadius);
        
    }
}