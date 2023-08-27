using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraChangerController : MonoBehaviour
{
    [SerializeField] EraManager eraManager;

    public List<EraChangerInput> eraInputs;
    EraType currentEra = EraType.Era1;
    protected void Update()
    {
        DetectInput();
    }


    private void DetectInput()
    {

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleEra();
            }
    }

    private void OnEraChangePressed(EraType eraType)
    {
        eraManager.ActivateEra(eraType);
    }
    
    private void ToggleEra()
    {
        // Toggle between Era1 and Era2
        if (currentEra == EraType.Era1)
        {
            currentEra = EraType.Era2;
        }
        else
        {
            currentEra = EraType.Era1;
        }

        eraManager.ActivateEra(currentEra);
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