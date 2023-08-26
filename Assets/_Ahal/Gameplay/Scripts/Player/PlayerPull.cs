using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPull : MonoBehaviour
{
    [SerializeField] private PlayerMovement2D playerMovement2D;
    [SerializeField] private LayerMask collideWithLayers;
    [SerializeField] Transform rayPoint;
    [SerializeField] Transform boxHolder;
    [SerializeField] float rayDistance;

    public bool IsGrabbingObject => grabbedObject != null;
    public Vector2 GrabbedObjectDirection => (grabbedObject.transform.position - transform.position).normalized;
    
    public GameObject grabbedObject;
    private int layerIndex;

    private Vector3 originalRayPointLocalPos;
    private Vector3 originalBoxHolderLocalPos;

    private bool _flipX;
    public bool FlipX
    {
        get => _flipX;
        set
        {
            if (value == _flipX) return;
            _flipX = value;
            var flipMultiplier = value ? -1 : 1;
            rayPoint.transform.SetLocalX(originalRayPointLocalPos.x * flipMultiplier);
            boxHolder.transform.SetLocalX(originalBoxHolderLocalPos.x * flipMultiplier);
        }
    }

    void Start()
    {
        layerIndex = LayerMask.NameToLayer("PullableObjects");

        originalRayPointLocalPos = rayPoint.localPosition;
        originalBoxHolderLocalPos = boxHolder.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var rayDir = _flipX ? -transform.right : transform.right;
        
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, rayDir, rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (grabbedObject == null)
                {
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabbedObject.transform.position = boxHolder.position;
                    grabbedObject.transform.SetParent(transform);
                }             
                else
                {
                    grabbedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    grabbedObject.transform.SetParent(null);
                    grabbedObject = null;
                }       
            }
        }      
        Debug.DrawRay(rayPoint.position, rayDir * rayDistance);
    }
    
    void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            // Check for collisions between the grabbed object and the environment.
            Collider2D[] colliders = grabbedObject.GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                Collider2D[] collidedWith = new Collider2D[2];
                Physics2D.OverlapCollider(collider, new ContactFilter2D(), collidedWith);
                var collidedObject = collidedWith.First(c =>c != null && c != collider && collideWithLayers.IsInLayer(c.gameObject.layer));
                if (collidedObject != null)
                {
                    playerMovement2D.Nudge(_flipX ? Vector2.right : Vector2.left);
                    break;
                }
            }
        }
    }
}
