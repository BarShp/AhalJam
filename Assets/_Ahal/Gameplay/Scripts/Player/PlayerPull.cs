using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPull : MonoBehaviour
{
    [SerializeField] Transform rayPoint;
    [SerializeField] Transform boxHolder;
    [SerializeField] float rayDistance;

    private GameObject grabbedObject;
    private int layerIndex;


    void Start()
    {
        layerIndex = LayerMask.NameToLayer("PullableObjects");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);

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
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    grabbedObject.transform.SetParent(null);
                    grabbedObject = null;
                }       
            }
        }      
        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }

}
