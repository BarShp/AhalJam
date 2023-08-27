using UnityEngine.Events;

public class ContinuousPressurePlate : TriggerUnityEventsBase
{
    public UnityEvent OnPressurePlateActivated;
    public UnityEvent OnPressurePlateStayTick;
    public UnityEvent OnPressurePlateDeactivated;
        
    protected void Start()
    {
        triggerEnterHandler += () => OnPressurePlateActivated?.Invoke();
        triggerStayHandler += () => OnPressurePlateStayTick?.Invoke();
        triggerExitHandler += () => OnPressurePlateDeactivated?.Invoke();
    }
}
