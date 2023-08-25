using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : InteractableComponent
{
    [SerializeField] UnityEvent OnInteractHandler = new UnityEvent();

    public override void OnInteract()
    {
        //Anyotherlogicappliestolever
        OnInteractHandler?.Invoke();
    }
}
