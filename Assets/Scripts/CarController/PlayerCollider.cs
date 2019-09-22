using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Player player;
    public Vector3 colliderNormal;

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

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Track"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                colliderNormal = contact.normal;
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        colliderNormal = Vector3.zero;
    }

    public void DisableCar(bool status = false, float timeDisabled = 0)
    {
        player.carController.SetDisabledStatus(status, timeDisabled);
    }
}
