
using UnityEngine;

[CreateAssetMenu(fileName = "NewWasteInfo", menuName = "WasteInfoSO", order = 1)]
public class WasteInfoSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }

    [Header("General")] 
    public string displayName;
    public WasteClassifications wasteClassification;
    public WasteMaterials wasteMaterial;
    public WasteType wasteType;
    public Sprite icon;

    private void OnValidate()
    {
        #if UNITY_EDITOR
            ID = name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}