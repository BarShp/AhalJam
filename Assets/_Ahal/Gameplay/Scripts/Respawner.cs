using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private Vector3 originalPosition;
    void Start()
    {
        originalPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = originalPosition;
    }
}
