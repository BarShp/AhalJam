using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraObjectController : MonoBehaviour
{

    private void Awake() {
        //Get RigidBody From Object
    }

    public void EnableObject() 
    {
        gameObject.SetActive(true);
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
}
