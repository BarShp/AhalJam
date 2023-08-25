using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PressurePlateBase : MonoBehaviour
{
    [SerializeField] private LayerMask pressureLayers;

    protected Action pressureStartedHandler;
    protected Action pressureStayHandler;
    protected Action pressureExitHandler;
    protected Collider2D collider;
    
    protected void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (!pressureLayers.IsInLayer(col.gameObject.layer)) return;
        pressureStartedHandler?.Invoke();
    }
    
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (!pressureLayers.IsInLayer(col.gameObject.layer)) return;
        pressureStayHandler?.Invoke();
    }
    
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (!pressureLayers.IsInLayer(col.gameObject.layer)) return;
        pressureExitHandler?.Invoke();
    }
}
