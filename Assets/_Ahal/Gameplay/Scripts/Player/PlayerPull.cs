using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPull : MonoBehaviour
{
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
}
