using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueMessage[] dialogueMessages;
    public Actor[] actors;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(dialogueMessages, actors);
    }

    private void Start() 
    {
        StartDialogue();
    }
}


[Serializable]
public class DialogueMessage
{
    public int ActorId;

    [TextArea(10,10)]
    public string Message;
}


[Serializable]
public class Actor
{
    public string name;
    public GameObject dialogueBox;
}

