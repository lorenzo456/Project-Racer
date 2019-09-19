using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Waypoint> waypoints = new List<Waypoint>();


    public int GetWayPointCount()
    {
        return waypoints.Count -1;
    }
    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Waypoint temp = transform.GetChild(i).GetComponent<Waypoint>();
            temp.waypointID = i;
            waypoints.Add(temp);

        }
    }
}
