using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
	{
		private PlayerGroundChecker _groundChecker;
		private PlayerAnimationManager _animationManager;
		private PlayerLook _playerLook;

		private Vector2 _moveInput;

		[Header("Function Options")] 
		[SerializeField] private bool canMove = true;
		[SerializeField] private bool canJump = true;
		[SerializeField] private bool canSprint = true;
		[SerializeField] private bool canCrouch = true;
		[SerializeField] private bool useStamina = false;
		
		[Header("Character Controller Parameters")]
		private float crouchHeight = 1f;
		private float _standHeight = 2f;
		private float standCamTargetHeight = 1.8f;
		private float crouchCamTargetHeight = 1f;
		public float StandHeight => _standHeight;
		
		[Header("Move Parameters")]
		[SerializeField] private FloatVariable WalkSpeed;
		[SerializeField] private FloatVariable SprintSpeed;
		[SerializeField] private FloatVariable CrouchSpeed;
		[SerializeField] private float SpeedChangeRate = 10.0f;
		[SerializeField] private FloatVariable rotationSmoothTime; //How fast the character turns to face movement direction for Tps
		[SerializeField] private BoolVariable _isAiming;
		[SerializeField] private FloatReference _rotationVelocity;
		[SerializeField] private Vector3Variable _tpsAimDirection;
		private CharacterController _controller;
		private CapsuleCollider _triggerCollider;
		private float _speed;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private float _targetMoveDirection = 1.0f;
		private bool _isSprintInput;

		[Header("Jump Parameters")]
		[SerializeField] private FloatVariable JumpHeight;
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		[SerializeField] private FloatVariable JumpTimeout; //Time required to pass before being able to jump again. Set to 0f to instantly jump again
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		[SerializeField] private FloatVariable FallTimeout; //Time required to pass before entering the fall state. Useful for walking down stairs
		[SerializeField] private FloatVariable Gravity ;
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		private bool _isJumpInput;

		[Header("Crouch Parameters")] 
		[SerializeField] private bool canStand = true;
		[SerializeField] private bool isCurrentlyCrouching = false;
		[SerializeField] private float crouchTransitionSpeed = 10f;
		[SerializeField] private float crouchTolerance = 0.001f;
		[SerializeField] private float crouchRayOffset = 0.5f; // must match char controller radius
		private bool _isCrouchInput;

		[Header("Stamina Parameters")]
		[SerializeField] private float maxStamina = 100;
		[SerializeField] private float minStaminaUsable = 10f;
		[SerializeField] private float staminaDrain = 5;
		[SerializeField] private float timeBeforeStart = 5;
		[SerializeField] private float staminaRegenValue = 2;
		private float timeSinceLastStaminaRegen  = 5f;
		private float currentStamina;
		

		[SerializeField] private Transform debugLookAt;

		private void OnEnable()
		{
			GameEventsManager.Instance.InputEvents.OnMoveEvent += MoveInputHandler;
			GameEventsManager.Instance.InputEvents.OnJumpEvent += JumpInputHandler;
			GameEventsManager.Instance.InputEvents.OnSprintEvent += SprintInputHandler;
			GameEventsManager.Instance.InputEvents.OnCrouchEvent += CrouchInputHandler;
		}
    
		private void OnDisable()
		{
			GameEventsManager.Instance.InputEvents.OnMoveEvent -= MoveInputHandler;
			GameEventsManager.Instance.InputEvents.OnJumpEvent -= JumpInputHandler;
			GameEventsManager.Instance.InputEvents.OnSprintEvent -= SprintInputHandler;
			GameEventsManager.Instance.InputEvents.OnCrouchEvent -= CrouchInputHandler;
		}

		private void Start()
		{
			_animationManager = GetComponent<PlayerAnimationManager>();
			_groundChecker = GetComponent<PlayerGroundChecker>();
			_playerLook = GetComponent<PlayerLook>();
			
			
			_controller = GetComponent<CharacterController>();
			_triggerCollider = GetComponent<CapsuleCollider>();
			_standHeight = _controller.height;
			_triggerCollider.height = _standHeight;
			standCamTargetHeight = _playerLook.CameraTargetObject.transform.position.y;
			crouchHeight = _standHeight / 2;
			crouchCamTargetHeight = standCamTargetHeight / 2;
			_controller.center = new Vector3(0, _standHeight / 2, 0);
			_triggerCollider.center = new Vector3(0, _standHeight / 2, 0);


			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			currentStamina = maxStamina;
			timeSinceLastStaminaRegen = timeBeforeStart;
			
		}

		private void Update()
		{
			Move();
			JumpAndGravity();
			Crouch();
			StaminaHandler();
		}

		// ***** PLAYER MOVE *****

		private void MoveInputHandler(Vector2 input)
		{
			_moveInput = input;
		}
		
		private void SprintInputHandler(bool input)
		{
			if (! canSprint) return;
			_isSprintInput = input;
		}
		
		private void CrouchInputHandler(bool input)
		{
			if (! canCrouch) return;
			_isCrouchInput = input;
		}
		
	private void Move()
	{
	    if (!canMove) return;

	    // Set target speed based on move speed, sprint speed, and joystick input magnitude
	    float targetSpeed = PlayerSpeed() * _moveInput.magnitude;

	    // If there is no input, set the target speed to 0
	    if (_moveInput == Vector2.zero) targetSpeed = 0.0f;

	    // Get the player's current horizontal velocity
	    float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

	    // Accelerate or decelerate to target speed
	    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);

	    // Round speed to 3 decimal places
	    _speed = Mathf.Round(_speed * 1000f) / 1000f;

	    // Blend animation based on target speed
	    _animationManager.AnimationBlend = Mathf.Lerp(_animationManager.AnimationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
	    if (_animationManager.AnimationBlend < 0.01f) _animationManager.AnimationBlend = 0f;

	    // Normalize input direction
	    Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

	    // Move the player based on the current view mode
	    switch (_playerLook.CurrentViewMode)
	    {
	        case PlayerLook.ViewMode.FirstPerson:
	        case PlayerLook.ViewMode.FirstPersonAiming:
	            MoveFirstPerson(inputDirection);
	            break;
	        case PlayerLook.ViewMode.ThirdPerson:
	        case PlayerLook.ViewMode.ThirdPersonAiming:
	            MoveThirdPerson(inputDirection);
	            break;
	        case PlayerLook.ViewMode.TopDown:
	            MoveTopDown(inputDirection);
	            break;
	    }

	    // Set the animator speed parameter
	    if (_animationManager.HasAnimator)
	    {
	        _animationManager.PlayerAnim.SetFloat(_animationManager.AnimIDPlayerSpeed, _animationManager.AnimationBlend);
	    }
	}

	private void MoveFirstPerson(Vector3 inputDirection)
	{
	    // If there is a move input, rotate the player when moving
	    if (_moveInput != Vector2.zero)
	    {
	        inputDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
	    }

	    // Move the player
	    _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
	}

	private void MoveThirdPerson(Vector3 inputDirection)
	{
		float rotationVelocity = _rotationVelocity.Value;

		if (_moveInput != Vector2.zero)
		{
			_targetMoveDirection = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _playerLook.MainCamera.transform.eulerAngles.y;
			
			if (_isAiming)
			{
				float targetFaceDirection = _playerLook.MainCamera.transform.eulerAngles.y;
					
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetFaceDirection, ref rotationVelocity, rotationSmoothTime);

				// Rotate to face the camera's forward direction (aiming)
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}
			else
			{
				// Regular movement logic, rotate to face the input direction relative to the camera position
				float targetFaceDirection = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _playerLook.MainCamera.transform.eulerAngles.y;

				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetFaceDirection, ref rotationVelocity, rotationSmoothTime);

				// Rotate to face the input direction
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}
		}
		else
		{
			if (_isAiming)
			{
				float targetFaceDirection = _playerLook.MainCamera.transform.eulerAngles.y;
					
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetFaceDirection, ref rotationVelocity, rotationSmoothTime);

				// Rotate to face the camera's forward direction (aiming)
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}
		}

		// Calculate movement direction
		Vector3 targetDirection = Quaternion.Euler(0.0f, _targetMoveDirection, 0.0f) * Vector3.forward;

		// Move the player
		_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
	}
	
	// private void MoveThirdPerson(Vector3 inputDirection)
	// {
	// 	float rotationVelocity = _rotationVelocity.Value;
	//
	// 	if (_moveInput != Vector2.zero)
	// 	{
	// 		_targetMoveDirection = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _playerLook.MainCamera.transform.eulerAngles.y;
	// 		
	// 		if (_isAiming)
	// 		{
	// 			float targetFaceDirection = _playerLook.MainCamera.transform.eulerAngles.y;
	// 				
	// 			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetFaceDirection, ref rotationVelocity, rotationSmoothTime);
	//
	// 			// Rotate to face the camera's forward direction (aiming)
	// 			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
	// 		}
	// 		else
	// 		{
	// 			// Regular movement logic, rotate to face the input direction relative to the camera position
	// 			float targetFaceDirection = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _playerLook.MainCamera.transform.eulerAngles.y;
	//
	// 			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetFaceDirection, ref rotationVelocity, rotationSmoothTime);
	//
	// 			// Rotate to face the input direction
	// 			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
	// 		}
	// 	}
	//
	// 	// Calculate movement direction
	// 	Vector3 targetDirection = Quaternion.Euler(0.0f, _targetMoveDirection, 0.0f) * Vector3.forward;
	//
	// 	// Move the player
	// 	_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
	// }
	
	

	private void MoveTopDown(Vector3 inputDirection)
	{
	    float rotationVelocity = _rotationVelocity.Value;

	    if (_moveInput != Vector2.zero || _isAiming)
	    {
	        _targetMoveDirection = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _playerLook.MainCamera.transform.eulerAngles.y;
	        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetMoveDirection, ref rotationVelocity, rotationSmoothTime);

	        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
	    }

	    Vector3 targetDirection = Quaternion.Euler(0.0f, _targetMoveDirection, 0.0f) * Vector3.forward;

	    // Move the player
	    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
	}

		private float PlayerSpeed()
		{
			return (_isSprintInput && !_isCrouchInput && !isCurrentlyCrouching) ? SprintSpeed : (_isCrouchInput || (isCurrentlyCrouching && !canStand)) ? CrouchSpeed : WalkSpeed;
		}

		// ***** PLAYER JUMP *****

		private void JumpInputHandler(bool val)
		{
			if (canJump)
			{
				_isJumpInput = val;
			}
		}
		
		private void JumpAndGravity()
		{
			if (_groundChecker.GetIsGrounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_animationManager.HasAnimator)
				{
					_animationManager.PlayerAnim.SetBool(_animationManager.AnimIDIsJumping, false);
					_animationManager.PlayerAnim.SetBool(_animationManager.AnimIDIsFreeFalling, false);
				}
				
				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_isJumpInput && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					
					// update animator if using character
					if (_animationManager.HasAnimator)
					{
						_animationManager.PlayerAnim.SetBool(_animationManager.AnimIDIsJumping, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_animationManager.HasAnimator)
					{
						_animationManager.PlayerAnim.SetBool(_animationManager.AnimIDIsFreeFalling, true);
					}
				}
				
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		
		
		// ***** PLAYER CROUCH *****

		private void Crouch()
		{
			//weird code but ok
			float height = _isCrouchInput ? crouchHeight : _standHeight;
			float camHeight = _isCrouchInput ? crouchCamTargetHeight : standCamTargetHeight;
			
			//Checks is there is something above
			// Calculate the starting position for the raycast with the offset
			Vector3 raycastStartPos = transform.position + transform.forward * crouchRayOffset;
			// Use the modified starting position for the raycast
			canStand = !Physics.Raycast(raycastStartPos, Vector3.up, 3f, ~LayerMask.GetMask("Player"));
			
			isCurrentlyCrouching = Math.Abs(_controller.height - crouchHeight) < crouchTolerance;
			
			if (canStand)
			{
				AdjustController(height, camHeight);
			}
			
			if (!canCrouch || !canStand) return;
			
			if (Math.Abs(_controller.height - height) > crouchTolerance) 
			{
				AdjustController(height, camHeight);
			}

			if (_animationManager.HasAnimator)
			{
				_animationManager.PlayerAnim.SetBool(_animationManager.AnimIDIsCrouching, ( _isCrouchInput || !canStand));
			}

		}

		private void AdjustController(float height, float camHeight)
		{
			float center = height / 2;

			_controller.height = Mathf.LerpUnclamped(_controller.height, height, crouchTransitionSpeed * Time.deltaTime);
			_controller.center = Vector3.LerpUnclamped(_controller.center, new Vector3(0, center, 0), crouchTransitionSpeed * Time.deltaTime);

			_playerLook.CameraTargetObject.transform.localPosition = Vector3.LerpUnclamped(_playerLook.CameraTargetObject.transform.localPosition, new Vector3(0, camHeight, 0), crouchTransitionSpeed * Time.deltaTime);
		}


		// ***** PLAYER STAMINA *****

		private void StaminaHandler()
		{
			if (!useStamina) return;

			if (timeSinceLastStaminaRegen <= timeBeforeStart)
			{
				timeSinceLastStaminaRegen += Time.deltaTime;
			}

			if (_isSprintInput && _moveInput != Vector2.zero)
			{
				currentStamina -= staminaDrain * Time.deltaTime;

				if (currentStamina < 0)
					currentStamina = 0;

				if (currentStamina <= 0)
				{
					canSprint = false;
					
					// Check if the stamina has reached the minimum usable threshold
					if (currentStamina >= minStaminaUsable)
					{
						// Allow sprinting again once the minimum usable stamina is reached
						canSprint = true;
					}
				}

				// Reset timeSinceLastStaminaRegen if the player is sprinting
				timeSinceLastStaminaRegen = 0f;
			}
			else if (!_isSprintInput && currentStamina < maxStamina )
			{
				// Check if enough time has passed since the last stamina regen
				if (timeSinceLastStaminaRegen >= timeBeforeStart)
				{
					currentStamina += staminaRegenValue * Time.deltaTime;

					if (currentStamina > maxStamina)
						currentStamina = maxStamina;

					// Set canSprint to true if stamina is greater than zero
					canSprint = currentStamina > 0 && currentStamina >= minStaminaUsable;
				}
			}
		}
		
		// ***** DEBUG *****
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(debugLookAt.position, 0.1f);
		}
	}