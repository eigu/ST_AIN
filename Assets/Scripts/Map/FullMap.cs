using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FullMap : MonoBehaviour
{
    public bool render = false;
    
    [Header("General")]
    [SerializeField] private MapManager _mapManager;
    [Tooltip("Object that will be scaled and the parent of the map graphics and icons.")]
    [SerializeField] private UIPanel _mapPanel;
    [SerializeField] private RectTransform _mapMask;
    [Tooltip("Object that will be parented to Map Mask")]
    [SerializeField] private RectTransform _mapBase; 
    [Tooltip("Child of Map Base that will be on the center.")]
    [SerializeField] private RectTransform _mapCenter;
    
    [Header("Icon Clamping")]
    [Tooltip("Used so that the clamped icons (highlighted) are not too close to the border.")]
    [SerializeField] private float _clampedIconPadding = 50f;

    [Header("Zooming")]
    [SerializeField] private float _zoomSpeed = 0.1f;
    [Tooltip("Farthest you can zoom in.")] [SerializeField] private float _minZoom = 1f;
    [Tooltip("Closest you can zoom in.")] [SerializeField] private float _maxZoom = 20f;
    
    [Header("Panning")]
    [SerializeField] private float _gamepadPanningSpeed = 3f;
    private bool _isMouse = true;
    private Vector2 _lastMousePosition;
    private Vector2 _lastGamepadDirection;
    private Vector2 _currentMousePosition;
    private bool _isPanning = false;

    private void OnEnable()
    {
        GameEventsManager.Instance.InputEvents.OnUIScrollEvent += ZoomMap;
        GameEventsManager.Instance.InputEvents.OnSelectStartEvent += StartMousePan;
        GameEventsManager.Instance.InputEvents.OnSelectEndEvent += StopMousePan;

        GameEventsManager.Instance.InputEvents.OnMousePanEvent += UpdateMousePosition;
        GameEventsManager.Instance.InputEvents.OnGamepadPanEvent += UpdateGamepadDirection;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.InputEvents.OnUIScrollEvent -= ZoomMap;
        GameEventsManager.Instance.InputEvents.OnSelectStartEvent -= StartMousePan;
        GameEventsManager.Instance.InputEvents.OnSelectEndEvent -= StopMousePan;
        
        GameEventsManager.Instance.InputEvents.OnMousePanEvent -= UpdateMousePosition;
        GameEventsManager.Instance.InputEvents.OnGamepadPanEvent -= UpdateGamepadDirection;
    }
    
    public void OpenMap()
    {
        GameEventsManager.Instance.UIEvents.OpenUIPanel(_mapPanel);

        // do when opening map
        
        // first, put back the icons on the orig position, because it might be clamped on the minimap
        foreach (var kvp in _mapManager.mapInfo.MapIcons)
        {
            kvp.Value.RectTransform.anchoredPosition = kvp.Value.originalPosition;
        }
        
        CenterMapOnIcon(_mapBase, _mapManager.CenterIcon.RectTransform);
    }

    private void Update()
    {
        if (!render) return;
        
        CancelPosition(_mapCenter, _mapBase, _mapBase.localScale);
            
        foreach (var kvp in _mapManager.mapInfo.MapIcons)
        {
            
            CancelParentScale(_mapBase.localScale, kvp.Value.RectTransform);
            
            //if the icon should be focused, clamp it so it is always visible, if it is visible, use the original position, if not it should be clamped
            if (kvp.Value.isHighlight) ClampIconPosition(kvp.Value);
        }
        
        if(_isMouse) HandleMapPanningMousePosition();
        if(!_isMouse) HandleMapPanningGamepadDirection();
        
    }

    void ClampIconPosition(MapIcon icon)
    {
        RectTransform iconRect = icon.RectTransform;
        
        iconRect.anchoredPosition = icon.originalPosition;
                
        Vector2 iconPosition = iconRect.anchoredPosition;

        var minimumDistanceX = ((_mapMask.rect.size.x - (iconRect.rect.size.x + _clampedIconPadding)) * 0.5f) / _mapBase.localScale.x;
        var minimumDistanceY = ((_mapMask.rect.size.y - (iconRect.rect.size.y + _clampedIconPadding)) * 0.5f) / _mapBase.localScale.y;
        
        Vector2 centerPosition = _mapCenter.anchoredPosition;

        iconPosition.x = Mathf.Clamp(
            iconPosition.x, 
            centerPosition.x - minimumDistanceX, 
            centerPosition.x + minimumDistanceX
        );
               
        iconPosition.y = Mathf.Clamp(
            iconPosition.y, 
            centerPosition.y - minimumDistanceY, 
            centerPosition.y + minimumDistanceY
        );
               
        iconRect.anchoredPosition = iconPosition;
    }

    void CancelPosition(RectTransform rect, RectTransform parent, Vector3 scale)
    {
        rect.anchoredPosition = new Vector2(-parent.anchoredPosition.x, -parent.anchoredPosition.y) / scale;
    }
    
    private void CancelParentScale(Vector3 parentScale, RectTransform obj)
    {
        obj.localScale = new Vector3(
            1 / parentScale.x,
            1 / parentScale.y,
            1 / parentScale.z
        );
    }

    private void CenterMapOnIcon(RectTransform map, RectTransform icon)
    {
        map.anchoredPosition = -icon.anchoredPosition * 2;
    }
    
    private void ZoomMap(float zoom)
    {
        if (zoom == 0)
            return;

        Vector2 mousePosition = _currentMousePosition;
        
        Vector2 zoomPoint = _isMouse ? _currentMousePosition : new Vector2(Screen.width / 2f, Screen.height / 2f);
   
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _mapBase, 
            zoomPoint, 
            null, 
            out Vector2 zoomPos
        );

        float currentMapScale = _mapBase.localScale.x;
        float zoomAmount = (zoom > 0 ? _zoomSpeed : -_zoomSpeed) * currentMapScale;
        float newScale = currentMapScale + zoomAmount;
        float clampedScale = Mathf.Clamp(newScale, _minZoom, _maxZoom);

        // Calculate zoom offset to center on mouse position
        Vector2 scaleDifference = zoomPos * (currentMapScale - clampedScale);
    
        _mapBase.localScale = Vector3.one * clampedScale;
        _mapBase.anchoredPosition += scaleDifference;
    }
    
    private void StartMousePan()
    {
        _lastMousePosition = Mouse.current.position.ReadValue();
        _isPanning = true;
    }
    
    private void StopMousePan()
    {
        _isPanning = false;
    }

    private void UpdateMousePosition(Vector2 pos)
    {
        if (!_isMouse && pos.magnitude > 0)
        {
            _isMouse = true;
        }
        
        if (_isMouse) _currentMousePosition = pos;
    }
    
    private void UpdateGamepadDirection(Vector2 dir)
    {
        if (_isMouse && dir.magnitude > 0)
        {
            _isMouse = false;
        }
        if (!_isMouse) _lastGamepadDirection = dir;
    }
    
    private void HandleMapPanningGamepadDirection()
    {
        _mapBase.anchoredPosition -= _lastGamepadDirection * _gamepadPanningSpeed;
    }
    
    void HandleMapPanningMousePosition()
    {
        // Perform panning while right mouse button is held
        if (_isPanning)
        {
            Vector2 panDelta = _currentMousePosition - _lastMousePosition;

            // Scale the pan movement based on current map scale
            _mapBase.anchoredPosition += panDelta;

            _lastMousePosition = _currentMousePosition;
        }
    }
}
