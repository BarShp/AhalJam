using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] bool moveOnStart;
    [SerializeField] List<Waypoint> waypoints;
    
    private int targetWaypointIndex = 0;

    private Waypoint currentWaypoint;
    private Waypoint targetWaypoint;
    private float lerpProgress = 0;
    private bool shouldMove;

    public void StartMoving() => shouldMove = true; 
    public void StopMoving() => shouldMove = false; 
    
    private void Start()
    {
        var firstWaypoint = waypoints[0];
        targetWaypointIndex = 1;

        transform.position = firstWaypoint.WayPointTransform.position;
        currentWaypoint = firstWaypoint;
        targetWaypoint = waypoints[targetWaypointIndex];

        if (moveOnStart)
        {
            StartMoving();
        }
    }

    void Update()
    {
        if (!shouldMove) return;
        
        if (lerpProgress >= 1)
        {
            targetWaypointIndex++;
            targetWaypointIndex %= waypoints.Count;

            currentWaypoint = targetWaypoint;
            targetWaypoint = waypoints[targetWaypointIndex];
            lerpProgress = 0;
        }

        lerpProgress += Time.deltaTime * targetWaypoint.Speed;
        transform.position = Vector2.Lerp(currentWaypoint.WayPointTransform.position, targetWaypoint.WayPointTransform.position, lerpProgress);
    }
}

[Serializable]
public class Waypoint
{
    public Transform WayPointTransform;
    public float Speed;
}