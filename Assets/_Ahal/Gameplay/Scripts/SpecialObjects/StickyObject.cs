using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StickyObject : MonoBehaviour
{
    [SerializeField] private LayerMask stickTo;

    private Rigidbody2D rb;
    private Rigidbody2D stickingObject;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var stickingObjectVelocity = stickingObject.velocity;
        stickingObjectVelocity.x += rb.velocity.x;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // TODO: Add check with Vector3.Dot to make sure the other object is above or something
        if (stickTo.IsInLayer(other.gameObject.layer))
        {
            stickingObject = other.gameObject.GetComponent<Rigidbody2D>();
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (stickingObject.gameObject == other.gameObject)
        {
            stickingObject = null;
        }
    }
}
