using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool startWaypoint;
    public int waypointID;


    public Vector3 GetSpawnPosition()
    {
        Vector3 spawnpoint;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
          //  Debug.DrawRay(transform.position, -transform.TransformDirection(Vector3.up) * hit.distance, Color.black);
        //    Debug.Log(hit.transform.name);
        }

        //spawnpoint = hit.transform.position;
        spawnpoint = new Vector3(transform.position.x, hit.transform.position.y + 2, transform.position.z);
       // spawnpoint.rotation = transform.rotation;
       // spawnpoint.localScale = new Vector3(1, 1, 1);
        return spawnpoint;
    }

    public Vector3 GetSpawnRotation()
    {
        Vector3 spawnRotation;

        spawnRotation = transform.forward;

        return spawnRotation;
    }

}
