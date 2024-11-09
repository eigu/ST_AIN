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

[CustomEditor(typeof(WasteInfoSO))]
public class WasteInfoSOTest : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        WasteInfoSO wasteInfoSO = (WasteInfoSO)target;

        wasteInfoSO.icon = EditorGUILayout.ObjectField(wasteInfoSO.icon, typeof(Sprite), false, GUILayout.Height(120),GUILayout.Width(120) ) as Sprite;


    }
}