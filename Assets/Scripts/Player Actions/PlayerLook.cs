using UnityEngine;
using Cinemachine;
using ScriptableObjectArchitecture;
using UnityEngine.UI;

public class PlayerLook : MonoBehaviour
{
    public enum ViewMode
    {
        FirstPerson,
        FirstPersonAiming,
        ThirdPerson,
        ThirdPersonAiming,
        TopDown
    }

    [Header("Control Parameters")]
    [SerializeField] private bool canLook = true;
    [SerializeField] private bool canSwitchView = true;
    [SerializeField] private bool canAim = true;
    [SerializeField] private bool canHideCursor = true;
    private Vector2 _mouseInput;
    
    [Header("Cameras")] 
    [SerializeField] private ViewMode currentViewMode = ViewMode.FirstPerson;
    [SerializeField] private GameObject[] _visualObjects;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera fpsNormalCam;
    [SerializeField] private CinemachineVirtualCamera fpsAimingCam;
    [SerializeField] private CinemachineVirtualCamera tpsNormalCam;
    [SerializeField] private CinemachineVirtualCamera tpsAimingCam;
    [SerializeField] private CinemachineVirtualCamera topDownCam;
    [SerializeField] private GameObject cameraTargetObject;
    
    [Header("Third Person View Parameters")] 
    [SerializeField] private FloatVariable maxLookUp;
    [SerializeField] private FloatVariable maxLookDown;
    [SerializeField] private float cameraAngleOverride = 0.0f; //Additional degrees to override the camera. Useful for fine tuning camera position when locked
    [SerializeField] private Vector3Variable _tpsAimDirection;
    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;
    private const float Threshold = 0.01f;
    
    [Header("Top Down View Parameters")] 
    [SerializeField] [Range(0, 90)] private float camAngle = 45f;
    [SerializeField] private bool lockYaw;
    [SerializeField] private float autoRotateSensitivity = 1f;
    private float currentYaw;

    [Header("Aim Parameters")]
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private FloatVariable normalAimSensitivity;
    [SerializeField] private FloatVariable focusedAimSensitivity;
    [SerializeField] private bool useCrossHair = false;

    [SerializeField] private Image crossHair;
    [SerializeField] private bool invertXLook = false;
    [SerializeField] private bool invertYLook = false;
    [SerializeField] private BoolVariable _isAiming;
    [SerializeField] private FloatVariable _rotationVelocity;
    private float _aimSensitivity = 1f;
    
    private bool _isAimInput;
    
    [SerializeField] private Transform debugCameraForwardArrow;
    public ViewMode CurrentViewMode => currentViewMode;
    public Camera MainCamera => mainCamera;
    public GameObject CameraTargetObject
    {
	    get => cameraTargetObject;
	    set => cameraTargetObject = value;
    }

    private void OnEnable()
    {
	    GameEventsManager.Instance.InputEvents.OnLookEvent += MouseInputHandler;
	    GameEventsManager.Instance.InputEvents.OnSwitchViewEvent += SwitchViewHandler;
	    GameEventsManager.Instance.InputEvents.OnAimEvent += AimInputHandler;
	    GameEventsManager.Instance.InputEvents.OnCursorEvent += CursorHandler;
	    GameEventsManager.Instance.InputEvents.OnSetGameEvent += HideCursorUI;
	    GameEventsManager.Instance.InputEvents.OnSetUIEvent += ShowCursorUI;
    }
    
    private void OnDisable()
    {
	    GameEventsManager.Instance.InputEvents.OnLookEvent -= MouseInputHandler;
	    GameEventsManager.Instance.InputEvents.OnSwitchViewEvent -= SwitchViewHandler;
	    GameEventsManager.Instance.InputEvents.OnAimEvent -= AimInputHandler;
	    GameEventsManager.Instance.InputEvents.OnCursorEvent -= CursorHandler;
	    GameEventsManager.Instance.InputEvents.OnSetGameEvent -= HideCursorUI;
	    GameEventsManager.Instance.InputEvents.OnSetUIEvent -= ShowCursorUI;
    }

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        _cinemachineTargetYaw = cameraTargetObject.transform.rotation.eulerAngles.y;
        
        ChangeView(currentViewMode);
        
        CursorHandler(false);
    }
    
    private void LateUpdate()
    {
	    CameraRotation();
	    Quaternion newRotation = Quaternion.Euler(debugCameraForwardArrow.rotation.eulerAngles.x, cameraTargetObject.transform.rotation.eulerAngles.y, debugCameraForwardArrow.rotation.eulerAngles.z);
	    debugCameraForwardArrow.rotation = newRotation;
    }


    private void MouseInputHandler(Vector2 input)
    {
	    _mouseInput = input;
    }
    
    private void CameraRotation()
	{
		//stop look
		if (!canLook) return;

		Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
		Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

		if (currentViewMode == ViewMode.FirstPerson || currentViewMode == ViewMode.FirstPersonAiming)
		{
			//FIRST PERSON
			if (_isAimInput)
			{
				ChangeView(ViewMode.FirstPersonAiming);
			}

			if (!_isAimInput)
			{
				ChangeView(ViewMode.FirstPerson);
			}

			// if there is an input
			if (_mouseInput.sqrMagnitude >= Threshold)
			{
				_cinemachineTargetPitch += _mouseInput.y * _aimSensitivity;
				if(canSwitchView) _cinemachineTargetYaw += _mouseInput.x * _aimSensitivity; //here to update if switching to TPS
				
				_rotationVelocity.Value = _mouseInput.x * _aimSensitivity;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, maxLookDown, maxLookUp);

				// Update Cinemachine camera target pitch
				cameraTargetObject.transform.localRotation = Quaternion.Euler(invertYLook ? _cinemachineTargetPitch : -_cinemachineTargetPitch, 0.0f, 0.0f);
				
				// rotate the player left and right
				transform.Rotate(Vector3.up * (invertXLook ? -_rotationVelocity : _rotationVelocity));
			}
		}
		else if (currentViewMode == ViewMode.ThirdPerson || currentViewMode == ViewMode.ThirdPersonAiming)
		{
			//THIRD PERSON
			if (_isAimInput)
			{
				ChangeView(ViewMode.ThirdPersonAiming);
			}

			if (!_isAimInput)
			{
				ChangeView(ViewMode.ThirdPerson);
			}

			// Sets the direction and position of aim
			Vector3 mouseWorldPosition = Vector3.zero;
			if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, aimColliderLayerMask)) // add interact mask
			{
				mouseWorldPosition = raycastHit.point;
				//if there is a problem in aiming, it may be because the sky dont have collider
				//use inverted normals and mesh collider
			}

			if (_isAiming)
			{
				Vector3 worldAimTarget = mouseWorldPosition;
				worldAimTarget.y = transform.position.y;
				_tpsAimDirection.Value = (worldAimTarget - transform.position).normalized;
			}

			// if there is an input and camera position is not fixed
			if (new Vector2(_mouseInput.x, _mouseInput.y).sqrMagnitude >= Threshold)
			{
				_cinemachineTargetYaw += _mouseInput.x * _aimSensitivity;
				_cinemachineTargetPitch += _mouseInput.y * _aimSensitivity;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, maxLookDown, maxLookUp);
			
			// Cinemachine will follow this target
			cameraTargetObject.transform.rotation = Quaternion.Euler(
				invertYLook ? _cinemachineTargetPitch : -_cinemachineTargetPitch + cameraAngleOverride,
				invertXLook ? -_cinemachineTargetYaw : _cinemachineTargetYaw, 0.0f);
			
		} else if (currentViewMode == ViewMode.TopDown)
		{
			if (!lockYaw)
			{
				// Get the forward direction of the transform
				Vector3 forward = transform.forward;
        
				// Calculate the yaw angle from the forward direction
				float targetYaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        
				// Lerp the target yaw to zero if it's not locked
				_cinemachineTargetYaw = Mathf.Lerp(_cinemachineTargetYaw, targetYaw, autoRotateSensitivity * Time.deltaTime);
			}
			
			_cinemachineTargetPitch = camAngle;
			
			// Cinemachine will follow this target
			cameraTargetObject.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
		}
		
	}
    
    private void ChangeView(ViewMode viewMode)
    {
	    fpsNormalCam.gameObject.SetActive(false);
	    fpsAimingCam.gameObject.SetActive(false);
	    tpsNormalCam.gameObject.SetActive(false);
	    tpsAimingCam.gameObject.SetActive(false);
	    topDownCam.gameObject.SetActive(false);

	    switch (viewMode)
	    {
		    case ViewMode.FirstPerson:
			    fpsNormalCam.gameObject.SetActive(true);
			    _aimSensitivity = normalAimSensitivity;
			    if (useCrossHair) crossHair.gameObject.SetActive(true);
					
			    //inherit rotation from tps rotation
			    transform.rotation = Quaternion.Euler(transform.rotation.x,_cinemachineTargetYaw,transform.rotation.z);
			    cameraTargetObject.transform.localRotation = Quaternion.Euler(invertYLook ? _cinemachineTargetPitch : -_cinemachineTargetPitch, 0.0f, 0.0f);
			    ToggleObjects(false);
			    break;
		    case ViewMode.FirstPersonAiming:
			    _isAiming.Value = true;
			    _aimSensitivity = focusedAimSensitivity;
			    fpsAimingCam.gameObject.SetActive(true);
			    break;
		    case ViewMode.ThirdPerson:
			    _isAiming.Value = false;
			    _aimSensitivity = normalAimSensitivity;
			    tpsNormalCam.gameObject.SetActive(true);
			    if (useCrossHair) crossHair.gameObject.SetActive(false);
			    ToggleObjects(true);
			    break;
		    case ViewMode.ThirdPersonAiming:
			    _isAiming.Value = true;
			    _aimSensitivity = focusedAimSensitivity;
			    tpsAimingCam.gameObject.SetActive(true);
			    if (useCrossHair) crossHair.gameObject.SetActive(true);
			    break;
		    case ViewMode.TopDown:
			    _isAiming.Value = false;
			    _aimSensitivity = normalAimSensitivity; //doesnt matter
			    if (useCrossHair) crossHair.gameObject.SetActive(false);
			    topDownCam.gameObject.SetActive(true);
			    ToggleObjects(true);
			    break;
	    }

	    currentViewMode = viewMode;
    }
    
    private void SwitchViewHandler()
    {
	    if (!canSwitchView) return;
			
	    if (currentViewMode == ViewMode.FirstPerson || CurrentViewMode ==ViewMode.FirstPersonAiming)
	    {
		    ChangeView(ViewMode.ThirdPerson);
	    }
	    else if (currentViewMode == ViewMode.ThirdPerson || CurrentViewMode ==ViewMode.ThirdPersonAiming )
	    {
		    ChangeView(ViewMode.FirstPerson);
		    
	    } 
	    //TODO: Make it cycle, automatically, add conditions if mode is included
    }
    
    private void CursorHandler(bool isShow)
    {
	    
	    if (!canHideCursor) return;
	    
	    if (isShow)
	    {
		    canLook = false;
		    Cursor.lockState = CursorLockMode.None;
		    Cursor.visible = true;
	    }
	    else
	    {
		    canLook = true;
		    Cursor.lockState = CursorLockMode.Locked;
		    Cursor.visible = false;
	    }
    }

    private void ShowCursorUI()
    {
	    canHideCursor = false;
	    
	    canLook = false;
	    Cursor.lockState = CursorLockMode.None;
	    Cursor.visible = true;
    }
    
    private void HideCursorUI()
    {
	    canHideCursor = true;
	    
	    canLook = true;
	    Cursor.lockState = CursorLockMode.Locked;
	    Cursor.visible = false;
    }

    private void AimInputHandler(bool val)
    {
	    if (!canAim) return;

	    _isAimInput = val;
	    
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
	    if (lfAngle < -360f) lfAngle += 360f;
	    if (lfAngle > 360f) lfAngle -= 360f;
	    return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    public void ToggleObjects(bool val)
    {
	    foreach (var obj in _visualObjects)
	    {
		    obj.SetActive(val);
	    }
    }
    
}