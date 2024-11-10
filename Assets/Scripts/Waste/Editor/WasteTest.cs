using Interact;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WasteInteract)), CanEditMultipleObjects]
public class WasteTest : Editor
{
    public override void OnInspectorGUI()
    {
        WasteInteract wasteInteract = (WasteInteract)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Rename Game Object to Info ID"))
        {
            wasteInteract.Rename();
        }
        
        
    }
}

