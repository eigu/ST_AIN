using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(PlayerInventoryHotbar))]
public class HotbarTest : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerInventoryHotbar playerInventoryHotbar = (PlayerInventoryHotbar)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Drop test"))
        {
            playerInventoryHotbar.CheckDrop();
        }
        
    }
}