using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public bool render = true;
    
    [Header("General")]
    [SerializeField] private MapManager _mapManager;
    [Tooltip("Object that will be scaled and the parent of the map graphics and icons.")]
    [SerializeField] private RectTransform _minimapBase; 
    [Tooltip("Parent of the RotateMask.")]
    [SerializeField] private RectTransform _minimapMask;
    [Tooltip("Object that will be rotated, also the parent of the Map Base.")]
    [SerializeField] private RectTransform _minimapRotateRectTransform;
    [Tooltip("If the map rotates where the world center reference (ex. player or cameraLookAt)).")]
    [SerializeField] private bool _rotateMap;
    private Matrix4x4 _transformationMatrix;
    
    //UI Boundaries
    [Header("UI Boundaries")]
    [Tooltip("If the Minimap Mask has a circular shape.")]
    [SerializeField] private bool circularMap;
    private Vector2 _minimapSize;
    private Vector2 _minimapMaskSize;
    private float _mapScale;
    
    // World boundaries
    [Header("World Boundaries")]
    [Tooltip("Align a sprite renderer image on top of the 3d world (align it perfectly as much as possible for accurate results). " +
             "Then click, get corners. Or you can input it manually.")]
    public SpriteRenderer mapReference;
    [SerializeField] private Vector3 topLeftCorner;
    [SerializeField] private Vector3 topRightCorner;
    [SerializeField] private Vector3 bottomRightCorner;
    [SerializeField] private Vector3 bottomLeftCorner;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        GetWorldSize();
        _minimapMaskSize = _minimapMask.rect.size;
        _minimapSize = _minimapBase.rect.size;
        CalculateTransformationMatrix();
        _minimapBase.localScale = new Vector3(_mapManager.defaultMinimapScale,_mapManager.defaultMinimapScale,_mapManager.defaultMinimapScale);
    }
    
    Vector2 GetWorldSize()
    {
        float worldSizeX = Vector2.Distance(new Vector2(topLeftCorner.x, 0), new Vector2(topRightCorner.x, 0));
        float worldSizeY = Vector2.Distance(new Vector2(0, topLeftCorner.z), new Vector2(0, bottomLeftCorner.z));

        return new Vector2(worldSizeX, worldSizeY);
    }

    void Update()
    {
        if (!render) return;

        if (_mapManager.CenterIcon != null)
        {
            CenterMapOnIcon();
            if(_rotateMap) RotateMap(); // IMPORTANT: clamping won't work on rectangular maps
            RenderMinimapIcons();
        }
    }

    void RenderMinimapIcons()
    {
        foreach (var kvp in _mapManager.mapInfo.MapIcons)
        {
            var worldToMapPos = WorldPositionToMapPosition(kvp.Key.position);
            
            if (kvp.Value.isMoving || kvp.Value.isHighlight)
            {
                kvp.Value.RectTransform.anchoredPosition = worldToMapPos;
            }

            kvp.Value.originalPosition = worldToMapPos;
            
            Vector3 parentScale = _minimapBase.localScale;

            if (kvp.Value.isRotating) RotateIconAsObjectRepresent(kvp.Value.IconRectTransform, kvp.Value.objectRepresent);
            if (kvp.Value.isCancelRotation) CancelParentRotation(_minimapBase.eulerAngles.z, kvp.Value.RectTransform);
            CancelParentScale(parentScale, kvp.Value.RectTransform);
            if (kvp.Value.isHighlight) ClampIconPosition(kvp.Value.RectTransform, parentScale, _mapManager.CenterIcon);
        }
    }

    private void CancelParentScale(Vector3 parentScale, RectTransform obj)
    {
        obj.localScale = new Vector3(
            1 / parentScale.x,
            1 / parentScale.y,
            1 / parentScale.z
        );
    }
    
    
    private void CancelParentRotation(float parentRotationZ, RectTransform obj)
    {
        obj.localEulerAngles = new Vector3(0, 0, -parentRotationZ);
    }

    private void ClampIconPosition(RectTransform icon, Vector3 parentScale, MapIcon centerIcon)
    {
        Vector2 iconPosition = icon.anchoredPosition;
        var minimumDistanceX = ((_minimapMaskSize.x - icon.rect.size.x) * 0.5f) / Mathf.Max(parentScale.x, parentScale.y);
        var minimumDistanceY = ((_minimapMaskSize.y - icon.rect.size.y) * 0.5f) / Mathf.Max(parentScale.x, parentScale.y);
        
        Vector2 centerPosition = centerIcon.RectTransform.anchoredPosition;

        if (!circularMap)
        {
            //rectangular
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
               
            icon.anchoredPosition = iconPosition;
        }
        else
        {
            //circular
            float distanceToPlayer = Vector2.Distance(centerPosition, iconPosition);
            
            if (distanceToPlayer > minimumDistanceX)
            {
                Vector2 direction = (iconPosition - centerPosition).normalized;
                iconPosition = centerPosition + direction * minimumDistanceX;
                icon.anchoredPosition = iconPosition;
            }
        }
    }
    
    public Vector2 WorldPositionToMapPosition(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z);
        return _transformationMatrix.MultiplyPoint3x4(pos);
    }

    void CalculateTransformationMatrix()
    {
        Vector3 centerWorld = new Vector3(
            (topLeftCorner.x + topRightCorner.x + bottomLeftCorner.x + bottomRightCorner.x) / 4,
            (topLeftCorner.z + topRightCorner.z + bottomLeftCorner.z + bottomRightCorner.z) / 4
        );

        Vector2 worldSize = GetWorldSize();
        
        var scaleRatio = new Vector2(_minimapSize.x / worldSize.x, _minimapSize.y / worldSize.y);
        Vector2 translation = new Vector2(-centerWorld.x, -centerWorld.y) * scaleRatio;
        
        _transformationMatrix = Matrix4x4.TRS(
            translation, 
            Quaternion.identity, 
            new Vector3(scaleRatio.x, scaleRatio.y, 1));
    }
    
    
    private void CenterMapOnIcon()
    {
        _mapScale = _minimapBase.localScale.x;
        _minimapBase.anchoredPosition = (-_mapManager.CenterIcon.RectTransform.anchoredPosition * _mapScale);
    }

    private void RotateIconAsObjectRepresent(RectTransform icon, Transform objectRepresent)
    {
        var rotation = objectRepresent.rotation.eulerAngles;
        icon.localRotation = Quaternion.AngleAxis(-rotation.y, Vector3.forward);
    }

    private void RotateMap()
    {
        var rotation = _mapManager.CenterIcon.objectRepresent.rotation.eulerAngles;
        _minimapRotateRectTransform.localEulerAngles = new Vector3(0,0,rotation.y);
    }

    public void GetCorners()
    {
        topLeftCorner = new Vector3(
            mapReference.bounds.center.x - mapReference.bounds.extents.x, 
            0, 
            mapReference.bounds.center.z + mapReference.bounds.extents.z);
      
        bottomRightCorner = new Vector3(
            mapReference.bounds.center.x + mapReference.bounds.extents.x, 
            0,
            mapReference.bounds.center.z - mapReference.bounds.extents.z);
    
        topRightCorner = new Vector3(
            mapReference.bounds.center.x + mapReference.bounds.extents.x, 
            0, 
            mapReference.bounds.center.z + mapReference.bounds.extents.z);
    
        bottomLeftCorner = new Vector3(
            mapReference.bounds.center.x - mapReference.bounds.extents.x, 
            0, 
            mapReference.bounds.center.z - mapReference.bounds.extents.z);
    }
    
}

[CustomEditor(typeof(Minimap))]
public class MinimapTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Minimap minimap = (Minimap)target;

        DrawDefaultInspector();

        if (GUILayout.Button("GetCorners"))
        {
            minimap.GetCorners();
        }
        
        
    }
}
