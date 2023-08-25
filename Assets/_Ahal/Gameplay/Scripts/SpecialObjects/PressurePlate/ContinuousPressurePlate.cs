using System;
using UnityEngine.Events;

public class ContinuousPressurePlate : PressurePlateBase
{
    public UnityEvent OnPressurePlateActivated;
    public UnityEvent OnPressurePlateStayTick;
    public UnityEvent OnPressurePlateDeactivated;
        
    protected void Start()
    {
        pressureStartedHandler += () => OnPressurePlateActivated?.Invoke();
        pressureStayHandler += () => OnPressurePlateStayTick?.Invoke();
        pressureExitHandler += () => OnPressurePlateDeactivated?.Invoke();
    }
}
