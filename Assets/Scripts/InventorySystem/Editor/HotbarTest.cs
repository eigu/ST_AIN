using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(Hotbar))]
public class HotbarTest : Editor
{
    public override void OnInspectorGUI()
    {
        Hotbar hotbar = (Hotbar)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Drop test"))
        {
            hotbar.CheckDrop();
        }
        
    }
}