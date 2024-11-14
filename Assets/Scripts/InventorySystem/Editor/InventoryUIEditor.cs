using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventoryUI inventoryUI = (InventoryUI)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Close Inventory"))
        {
            inventoryUI.CloseInventory();
        }
        
    }
}