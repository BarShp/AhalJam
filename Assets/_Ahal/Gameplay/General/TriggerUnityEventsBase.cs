using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TriggerUnityEventsBase : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayers;

    protected Action triggerEnterHandler;
    protected Action triggerStayHandler;
    protected Action triggerExitHandler;
    protected Action collisionEnterHandler;
    protected Collider2D triggerCollider;
    
    protected void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
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

    protected void OnCollisionEnter2D(Collision2D col) {
        if (!triggerLayers.IsInLayer(col.gameObject.layer)) return;
        collisionEnterHandler?.Invoke();        
    }
}
