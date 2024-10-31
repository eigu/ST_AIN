using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    #region Instance

    private static DialogueManager _instance;

    public static DialogueManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(this);
        }
    }

    #endregion

    public DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;

    private IEnumerator StartTypewriterEffect(string startNode)
    {
        yield return new WaitForSeconds(0.1f); 
        dialogueRunner.StartDialogue(startNode);
    }

    public void OpenDialogue(string startNode)
    {
        StartCoroutine(StartTypewriterEffect(startNode));
    }

    public void SendDialogueTest()
    {
        OpenDialogue("Start_Test");
    }
    
    
}
