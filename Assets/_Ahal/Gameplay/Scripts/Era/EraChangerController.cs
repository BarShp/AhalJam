using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraChangerController : MonoBehaviour
{
    [SerializeField] EraManager eraManager;

    public List<EraChangerInput> eraInputs;

    protected void Update()
    {
        DetectInput();
    }


    private void DetectInput()
    {
        foreach (EraChangerInput eraInput in eraInputs)
        {
            if (Input.GetKeyDown(eraInput.InputKeyCode))
            {
                OnEraChangePressed(eraInput.EraType);
            }
        }
    }

    private void OnEraChangePressed(EraType eraType)
    {
        eraManager.ActivateEra(eraType);
    }
}

public enum EraType
{
    Era1,
    Era2
}

[Serializable]
public class EraChangerInput
{
    public EraType EraType;
    public KeyCode InputKeyCode;
}