using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EraObjectController : MonoBehaviour
{
    public bool canRespawn = true;
    private void Awake() {
        //Get RigidBody From Object
    }

    public void EnableObject() 
    {
        if (canRespawn) 
        {
            gameObject.SetActive(true);
        }
        //SetActiveTrue
        //Animation
        //Timer for X
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
        //DisableCollision?
        //Animation
        //Timer for X
        //SetActiveFalse
    }

    public void PreventRespawn()
    {
        canRespawn = false;
    }
}
