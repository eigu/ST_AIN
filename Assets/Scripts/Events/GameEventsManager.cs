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
    public PlayerInteractEvents PlayerInteractEvents;
    public HotbarEvents HotbarEvents;
    public WasteEvents WasteEvents;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            InputEvents = new InputEvents();

            PlayerInteractEvents = new PlayerInteractEvents();
            
            HotbarEvents = new HotbarEvents();
            
            WasteEvents = new WasteEvents();
        }
        else
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
    }

}
