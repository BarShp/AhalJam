using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform transformToFollow;

    public void Update()
    {
        transform.position = transformToFollow.position;
    }
}
