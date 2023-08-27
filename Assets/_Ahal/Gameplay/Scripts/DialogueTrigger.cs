using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueMessage[] dialogueMessages;
    public Actor[] actors;
    private bool isTriggered = false;
    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(dialogueMessages, actors);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && isTriggered == false) 
        {
            other.gameObject.GetComponent<PlayerMovement2D>().DisablePlayerControls();
            StartDialogue();
            isTriggered = true;
        }
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

