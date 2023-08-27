using UnityEngine.Events;

public class SinglePressPressurePlate : TriggerUnityEventsBase
{
    public UnityEvent OnPressurePlateActivated;
    public UnityEvent OnPressurePlateStayTick;
    public UnityEvent OnPressurePlateDeactivated;
        
    protected void Start()
    {
        triggerEnterHandler += PressureStartedHandler;
    }

    private void PressureStartedHandler()
    {
        OnPressurePlateActivated?.Invoke();
        triggerCollider.enabled = false;
    }
}
