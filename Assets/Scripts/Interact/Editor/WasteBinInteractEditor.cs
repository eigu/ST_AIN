using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(WasteBinInteract)), CanEditMultipleObjects]
public class WasteBinInteractEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WasteBinInteract wasteBin = (WasteBinInteract)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Open Inventory"))
        {
            wasteBin.OpenInventory();
        }
        
    }
}