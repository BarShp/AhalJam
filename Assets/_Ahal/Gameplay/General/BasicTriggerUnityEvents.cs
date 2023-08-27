using UnityEngine.Events;

public class BasicTriggerUnityEvents : TriggerUnityEventsBase
{
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;
    public UnityEvent OnCollision;
    
    protected void Start()
    {
        triggerEnterHandler += () => OnEnter?.Invoke();
        triggerStayHandler += () => OnStay?.Invoke();
        triggerExitHandler += () => OnExit?.Invoke();
        collisionEnterHandler += () => OnCollision?.Invoke();
    }
}
