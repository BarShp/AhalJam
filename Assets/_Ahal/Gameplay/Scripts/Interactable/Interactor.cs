using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    InteractableComponent currentInteractableComponent;

    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (currentInteractableComponent != null)
            {
                currentInteractableComponent.OnInteract();
            }
            else
            {
                print("No object in range");
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if (!other.TryGetComponent<InteractableComponent>(out var otherInteractableComponent)) return;
        currentInteractableComponent = otherInteractableComponent;
    }

    protected void OnTriggerExit2D(Collider2D other) {
        currentInteractableComponent = null;
    }
}
