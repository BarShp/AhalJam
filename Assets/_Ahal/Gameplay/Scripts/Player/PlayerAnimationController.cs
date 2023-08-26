using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animatorComponent;

    [SerializeField] private string idleBoolParameter;
    [SerializeField] private string runBoolParameter;

    private List<string> allBoolParameters;
    private string currentState = null;
    
    protected void Start()
    {
        allBoolParameters = new List<string>()
        {
            idleBoolParameter,
            runBoolParameter
        };
    }

    public void SetIdle() => SetBoolState(idleBoolParameter);
    public void SetRun() => SetBoolState(runBoolParameter);

    private void SetBoolState(string stateToEnable)
    {
        if (stateToEnable == currentState) return;
        
        foreach (var boolParameter in allBoolParameters)
        {
            animatorComponent.SetBool(boolParameter, boolParameter == stateToEnable);
        }
        currentState = stateToEnable;
    }
}
