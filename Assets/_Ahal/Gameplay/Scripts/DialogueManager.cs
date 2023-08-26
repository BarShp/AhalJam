using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject AgentDialogue;
    [SerializeField] GameObject SphinxDialogue;
    public static bool isActive = false;
    // Start is called before the first frame update
    DialogueMessage[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    int currentActorId;

    public void OpenDialogue(DialogueMessage[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        DisplayMessage();
    }

    private void DisplayMessage()
    {
        DialogueMessage messageToDisplay = currentMessages[activeMessage];
        Actor actorToDisplay = currentActors[messageToDisplay.ActorId];
        if (currentActorId != messageToDisplay.ActorId)
        {
            for (int i = 0; i < currentActors.Length; i++)
            {
                currentActors[i].dialogueBox.SetActive(messageToDisplay.ActorId == i);
            }
        }

        actorToDisplay.dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = messageToDisplay.Message;
        currentActorId = messageToDisplay.ActorId;
    }

    private void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            currentActors[currentActorId].dialogueBox.SetActive(false);
            GameObject.Find("Player").GetComponent<PlayerMovement2D>().disableControls = false;
        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.V) && isActive)
        {
            NextMessage();
        }
    }
}
