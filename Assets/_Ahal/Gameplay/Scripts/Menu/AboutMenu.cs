using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AboutMenu : MonoBehaviour
{
    public UnityEvent onBackPress;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onBackPress?.Invoke();
        }

        
    }
}
