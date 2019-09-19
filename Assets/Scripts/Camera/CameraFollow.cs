using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;


    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;


    }
    private void Update()
    {
        // Calculate the journey length.

    }
    private void LateUpdate()
    {
        Vector3 forward = (target.TransformDirection(-Vector3.forward) * 5) + offset;
        Vector3 desiredLocation = target.position + forward;

        journeyLength = Vector3.Distance(transform.position, desiredLocation);
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        transform.position = target.position + forward;

        Vector3 pos = target.position - transform.position;
        var newRot = Quaternion.LookRotation(pos);

        transform.LookAt(target);
        Debug.DrawRay(target.position, forward, Color.green);
    }
}
