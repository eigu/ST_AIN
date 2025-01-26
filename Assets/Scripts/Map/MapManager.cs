using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapDataSO mapInfo;
    
    [SerializeField] private Minimap _minimapScript;
    [SerializeField] private FullMap _fullMapScript;
    
    [SerializeField] private RectTransform _minimapParent;
    [SerializeField] private RectTransform _fullmapParent;
    
    [SerializeField] private RectTransform _mapBase;

    private MapIcon centerIcon; //player
    public MapIcon CenterIcon => centerIcon;

    public float defaultMinimapScale = 1;
    public float defaultFullMapScale = 1;

    private Vector3 _lastMinimapRotation;
    
    private void OnEnable()
    {
        GameEventsManager.Instance.InputEvents.OnOpenMapEvent += OpenMap;
        GameEventsManager.Instance.UIEvents.RegisterMapWorldObjectIconEvent += RegisterWorldObject;
        GameEventsManager.Instance.UIEvents.UnregisterMapWorldObjectIconEvent += UnregisterWorldObject;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InputEvents.OnOpenMapEvent -= OpenMap;
        GameEventsManager.Instance.UIEvents.RegisterMapWorldObjectIconEvent -= RegisterWorldObject;
        GameEventsManager.Instance.UIEvents.UnregisterMapWorldObjectIconEvent -= UnregisterWorldObject;
    }

    void UnregisterWorldObject(Transform objectReference)
    {
        if (mapInfo.MapIcons.ContainsKey(objectReference) && objectReference.gameObject.activeInHierarchy)
        {
            Destroy(mapInfo.MapIcons[objectReference].gameObject);
            mapInfo.MapIcons.Remove(objectReference);
        }
    }
    
    void RegisterWorldObject(Transform obj, MapIconDataSO mapIconInfo, string overrideName, bool isHighlight, float overrideScale)
    {
        var minimapIcon = mapInfo.MapRegisterInstanceWorldObject(
            _mapBase, 
            obj, 
            _minimapScript.WorldPositionToMapPosition(obj.position), 
            mapIconInfo,
            overrideName,
            isHighlight,
            overrideScale
        );

        if (mapIconInfo.isPlayer)
        {
            centerIcon = minimapIcon;
        }
        
    }

    void OpenMap()
    {
        GameEventsManager.Instance.InputEvents.SetUI();
        
        _minimapScript.render = false;
        _fullMapScript.render = true;
        _mapBase.SetParent(_fullmapParent);
        _mapBase.localScale = new Vector3(defaultFullMapScale, defaultFullMapScale, defaultFullMapScale);
        _lastMinimapRotation = _mapBase.eulerAngles;
        _mapBase.eulerAngles = Vector3.zero;
        foreach (var kvp in mapInfo.MapIcons)
        {
            kvp.Value.RectTransform.eulerAngles = Vector3.zero;
        }
        
        _fullMapScript.OpenMap();
        
        
    }

    // used in editor
    public void CloseMap()
    {
        _minimapScript.render = true;
        _fullMapScript.render = false;
        _mapBase.SetParent(_minimapParent);
        _mapBase.eulerAngles = _lastMinimapRotation;
        _mapBase.localScale = new Vector3(defaultMinimapScale, defaultMinimapScale, defaultMinimapScale);
        
        foreach (var kvp in mapInfo.MapIcons)
        {
            // should not be really on a foreach loop since only 1 icon will have an active label
            kvp.Value.labelContainer.SetActive(false);
        }
    }
    
}
