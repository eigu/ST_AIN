using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueCubeTest : MonoBehaviour
{
    public string startNodeName;
    public Renderer ren;
    public BarkUI bark;

    public List<DialogueCommandHandler> commandHandlers = new List<DialogueCommandHandler>();
    
    private void Start()
    {
        foreach (var commandHandler in commandHandlers)
        {
            DialogueManager.Instance.dialogueRunner.AddCommandHandler(commandHandler.commandName, () => commandHandler.del.Invoke());
            //make this one by one so that YarnSpinner recognize the commandName being mentioned
        }
    }

    public void Talk()
    {
        bark.StopBark();
        DialogueManager.Instance.OpenDialogue(startNodeName);
    }

    public void TurnRed()
    {
        ren.material.color = Color.red;
    }
    
    public void TurnBlue()
    {
        ren.material.color = Color.blue;
    }

    public void EndDialogue()
    {
        bark.StartShowingBark();
    }
    
    public void HelloWorld()
    {
        Debug.Log("Hello world!");
    }
    
    
}

[Serializable]
public struct DialogueCommandHandler
{
    public string commandName;
    public UnityEvent del;
}
