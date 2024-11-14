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
 public QuestEvents QuestEvents;
    public PlayerInteractEvents PlayerInteractEvents;
     public PlayerInteractEvents PlayerInteractEvents;
    public HotbarEvents HotbarEvents;
    public WasteEvents WasteEvents;
    public InventoryEvents InventoryEvents;
    
    public PathFindingEvents PathFindingEvents;
    
    public BatteryEvents BatteryEvents;
    
    public DialogueEvents DialogueEvents;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            InputEvents = new InputEvents();
            
            QuestEvents = new QuestEvents();
            
            PlayerInteractEvents = new PlayerInteractEvents();
            
            HotbarEvents = new HotbarEvents();
            
            WasteEvents = new WasteEvents();
            
            InventoryEvents = new InventoryEvents();
            
            PathFindingEvents = new PathFindingEvents();
            
            BatteryEvents = new BatteryEvents();
            
            DialogueEvents = new DialogueEvents();
            
        }
        else
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
    }

}
