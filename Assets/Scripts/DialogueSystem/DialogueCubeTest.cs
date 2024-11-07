using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueCubeTest : MonoBehaviour
{
    [SerializeField] [Tooltip("Yarn Spinner node name.")]
    private string _startNodeName;
    [SerializeField] [Tooltip("Disable bark when dialogue is playing.")]
    private BarkUI _bark;
    [SerializeField] [Tooltip("For Testing Purposes. Will change color based on commands.")]
    private Renderer _ren;
    

    public List<DialogueCommandHandler> commandHandlers = new List<DialogueCommandHandler>();
    
    public void StartDialogue()
    {
        
        foreach (var commandHandler in commandHandlers)
        {
            GameEventsManager.Instance.DialogueEvents.AddDialogueCommand(commandHandler);
            //make this one by one so that YarnSpinner recognize the commandName being mentioned
        }

        if (_bark) _bark.StopBark();
       
        GameEventsManager.Instance.DialogueEvents.StartDialogue(_startNodeName);
    }

    public void TurnRed()
    {
        if (_ren) _ren.material.color = Color.red;
    }
    
    public void TurnBlue()
    {
        if (_ren) _ren.material.color = Color.blue;
    }

    public void EndDialogue()
    {
        if (_bark) _bark.StartShowingBark();
        
        foreach (var commandHandler in commandHandlers)
        {
            GameEventsManager.Instance.DialogueEvents.RemoveDialogueCommand(commandHandler);
        }
    }
    
    
    
}

[Serializable]
public struct DialogueCommandHandler
{
    public string commandName;
    public UnityEvent del;
}
