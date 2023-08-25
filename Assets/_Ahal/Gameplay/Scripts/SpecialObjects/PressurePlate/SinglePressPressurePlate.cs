using System;
using UnityEngine.Events;

public class SinglePressPressurePlate : PressurePlateBase
{
    public UnityEvent OnPressurePlateActivated;
        
    protected void Start()
    {
        pressureStartedHandler += PressureStartedHandler;
    }

    private void PressureStartedHandler()
    {
        OnPressurePlateActivated?.Invoke();
        collider.enabled = false;
    }
}
