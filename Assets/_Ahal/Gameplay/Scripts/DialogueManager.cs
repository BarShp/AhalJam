using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] bool enablePlayerControls = true;
    public static bool isActive = false;
    // Start is called before the first frame update
    DialogueMessage[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    int currentActorId;
    public UnityEvent OnDialogueFinished;
    

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
        if (currentActorId != messageToDisplay.ActorId || activeMessage == 0)
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
            OnDialogueFinished?.Invoke();
            isActive = false;
            currentActors[currentActorId].dialogueBox.SetActive(false);
            if (enablePlayerControls)
            {
                GameObject.Find("Player").GetComponent<PlayerMovement2D>().disableControls = false;
            }
        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && isActive)
        {
            NextMessage();
        }
    }
}
