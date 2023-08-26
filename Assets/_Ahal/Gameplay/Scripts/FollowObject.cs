using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform transformToFollow;

    public void Update()
    {
        // Ignore z in this script, too lazy to write it better now, quick hack
        var newPosition = transformToFollow.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
