using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Dialogue Runner is attached in the dialogue UI object.")]
    private DialogueRunner _dialogueRunner;

    private void OnEnable()
    {
        GameEventsManager.Instance.DialogueEvents.OnStartDialogueEvent += StartDialogue;
        GameEventsManager.Instance.DialogueEvents.OnAddDialogueCommandEvent += AddCommandsToRunner;
        GameEventsManager.Instance.DialogueEvents.OnRemoveDialogueCommandEvent += RemoveCommandsFromRunner;
    }
    
    private void OnDisable()
    {
        GameEventsManager.Instance.DialogueEvents.OnStartDialogueEvent -= StartDialogue;
        GameEventsManager.Instance.DialogueEvents.OnAddDialogueCommandEvent -= AddCommandsToRunner;
        GameEventsManager.Instance.DialogueEvents.OnRemoveDialogueCommandEvent -= RemoveCommandsFromRunner;
    }

    private IEnumerator StartDialogueWithDelay(string startNode)
    {
        yield return new WaitForSeconds(0.1f); 
        GameEventsManager.Instance.InputEvents.SetUI();
        _dialogueRunner.StartDialogue(startNode);
    }

    private void StartDialogue(string startNode)
    {
        StartCoroutine(StartDialogueWithDelay(startNode));
    }

    private void AddCommandsToRunner(DialogueCommandHandler dialogueCommandHandler)
    {
        _dialogueRunner.AddCommandHandler(dialogueCommandHandler.commandName, () => dialogueCommandHandler.del.Invoke());
    }
    
    private void RemoveCommandsFromRunner(DialogueCommandHandler dialogueCommandHandler)
    {
        _dialogueRunner.RemoveCommandHandler(dialogueCommandHandler.commandName);
    }
    
    //testing
    public void SendDialogueTest()
    {
        StartDialogue("Start_Test");
    }


}
