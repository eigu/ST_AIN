using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapData", menuName = "Map Data")]
public class MapDataSO : ScriptableObject
{
    public MapIcon mapIconPrefab;

    private Dictionary<Transform, MapIcon> _mapIcons = new Dictionary<Transform, MapIcon>();

    public Dictionary<Transform, MapIcon> MapIcons => _mapIcons;

    private void Awake()
    {
        _mapIcons = new Dictionary<Transform, MapIcon>();
    }

    /// <summary>
    /// Used by Map Manager to instantiate and add it to the icon dictionary
    /// </summary>
    public MapIcon MapRegisterInstanceWorldObject(RectTransform parent, Transform obj, Vector2 worldToMapPos, MapIconDataSO mapIconInfo,
        string overrideName, bool isHighlight, float overrideScaleMultiplier)
    {
        var minimapIcon = Instantiate(mapIconPrefab);

        minimapIcon.transform.SetParent(parent);

        minimapIcon.objectRepresent = obj;

        minimapIcon.Image.sprite = mapIconInfo.minimapIcon;
        minimapIcon.isHighlight = isHighlight;
        minimapIcon.isMoving = mapIconInfo.isMoving;
        minimapIcon.isRotating = mapIconInfo.isRotating;
        minimapIcon.iconName = overrideName != "" ? overrideName : mapIconInfo.iconDefaultName;
        minimapIcon.isCancelRotation = mapIconInfo.isCancelRotation;
        minimapIcon.isClickable = mapIconInfo.isClickable;
        minimapIcon.overrideScaleMultiplier = overrideScaleMultiplier;
        minimapIcon.ApplyScale();

        Debug.Log($"Registering {obj.name}");
        minimapIcon.RectTransform.anchoredPosition = worldToMapPos;

        _mapIcons.Add(obj, minimapIcon);

        return minimapIcon;
    }

}