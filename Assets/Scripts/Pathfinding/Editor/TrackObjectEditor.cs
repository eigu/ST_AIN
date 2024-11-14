using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackObject))]
public class TrackObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TrackObject trackObject = (TrackObject)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Start Track Me"))
        {
            trackObject.StartTrack();
        }
        
        if (GUILayout.Button("Stop Tracking Me"))
        {
            trackObject.StopTrack();
        }
        
    }
}