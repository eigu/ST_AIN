using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WasteInfoSO), true)]
public class WasteInfoSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WasteInfoSO wasteInfoSo = (WasteInfoSO)target;

        GUILayout.Space(25f);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        wasteInfoSo.icon = EditorGUILayout.ObjectField(wasteInfoSo.icon, typeof(Sprite), false, GUILayout.Height(120), GUILayout.Width(120)) as Sprite;

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(25f);   
        
        DrawPropertiesExcluding(serializedObject, "icon");

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}