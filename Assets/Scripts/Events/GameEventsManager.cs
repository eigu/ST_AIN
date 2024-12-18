using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    private static GameEventsManager _instance;

    public static GameEventsManager Instance
    {
        get { return _instance; }
    }
    
    //events
    public InputEvents InputEvents;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            InputEvents = new InputEvents();
        }
        else
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
    }

}
