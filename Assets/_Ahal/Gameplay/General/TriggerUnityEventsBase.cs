using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TriggerUnityEventsBase : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayers;

    protected Action triggerEnterHandler;
    protected Action triggerStayHandler;
    protected Action triggerExitHandler;
    protected Collider2D collider;
    
    protected void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (!triggerLayers.IsInLayer(col.gameObject.layer)) return;
        triggerEnterHandler?.Invoke();
    }
    
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (!triggerLayers.IsInLayer(col.gameObject.layer)) return;
        triggerStayHandler?.Invoke();
    }
    
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (!triggerLayers.IsInLayer(col.gameObject.layer)) return;
        triggerExitHandler?.Invoke();
    }
}
