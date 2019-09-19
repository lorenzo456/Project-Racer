using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            player.addCurrentSpawnIndex(other.GetComponent<Waypoint>().waypointID);
        }
    }
    
    public void DisableCar(bool status = false, float timeDisabled = 0)
    {
        player.carController.SetDisabledStatus(status, timeDisabled);
    }
}
