using System;

public class DialogueEvents
{
    public event Action<string> OnStartDialogueEvent;
    public event Action<DialogueCommandHandler> OnAddDialogueCommandEvent;
    public event Action<DialogueCommandHandler> OnRemoveDialogueCommandEvent;

    public void StartDialogue(string nodeName)
    {
        OnStartDialogueEvent?.Invoke(nodeName);
    }
    
    public void AddDialogueCommand(DialogueCommandHandler dialogueCommandHandler)
    {
        OnAddDialogueCommandEvent?.Invoke(dialogueCommandHandler);
    }
    
    public void RemoveDialogueCommand(DialogueCommandHandler dialogueCommandHandler)
    {
        OnRemoveDialogueCommandEvent?.Invoke(dialogueCommandHandler);
    }
}