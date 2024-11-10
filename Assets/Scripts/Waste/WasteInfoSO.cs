
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWasteInfo", menuName = "WasteInfoSO", order = 1)]
public class WasteInfoSO : ScriptableObject
{ 
    [field: SerializeField] public string ID { get; private set; }

    [Header("General")] 
    public string displayName;
    public Sprite icon;
    public bool isStackable; //in inventory
    public string description; 
    
    
    [Header("Waste Parameters")] 
    public WasteClassifications wasteClassification;
    public WasteMaterials wasteMaterial;
    public WasteType wasteType;
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        ID = name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}