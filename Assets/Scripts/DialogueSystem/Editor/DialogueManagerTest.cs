using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerTest : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueManager manager = (DialogueManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Test"))
        {
            manager.SendDialogueTest();
        }
        
        
    }
}

[CustomEditor(typeof(DialogueCubeTest))]
public class DialogueCubeTestTest : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueCubeTest cube = (DialogueCubeTest)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Test"))
        {
            cube.Talk();
        }
        
        
    }
}