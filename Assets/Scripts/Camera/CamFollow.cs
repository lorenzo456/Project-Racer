using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public float speed;
    public float minDistance;
    public float distance;
    public float maxDistance;
    public float height;
    public Transform target;

    public CarController carController;

    void LateUpdate()
    {
        //StandardMovement();
        //StaticMovement();
        Movement();
    }

    void Movement()
    {
        if (distance > maxDistance)
        {
            if(maxDistance - distance < .5f)
            {
                distance = maxDistance;
            }
            else
            {
                distance = Mathf.Lerp(distance, -maxDistance, Time.deltaTime * speed);
            }
        }
        /*
        Vector3 desiredPos = target.position + (target.forward * -distance) + (target.up * height);
        transform.position = desiredPos;
        //transform.forward = target.forward;
        */

        Vector3 desiredPos = target.position + (target.forward * -distance) + (target.up * height);

        float xDesiredPos = Mathf.Lerp(transform.position.x, desiredPos.x, Time.deltaTime * 10);
        transform.position = new Vector3(xDesiredPos, desiredPos.y, desiredPos.z);

        float pro = (carController.GetCurrentSpeed() / carController.topSpeed) * 100;
        float dist = pro * (maxDistance / 100);
        if(dist < minDistance)
        {
            dist = minDistance;
        }

        distance = dist;
        LookAt();
    }




    Vector3 velocity = Vector3.zero;
    void StaticMovement()
    {

    Vector3 desiredPos = target.position + (target.forward * -distance) + (target.up * height);
        //transform.position = new Vector3(desiredPos.x, desiredPos.y, Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime * speed));

        // transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, Time.deltaTime * speed);

        //transform.position = Vector3.MoveTowards(transform.position,desiredPos, Time.deltaTime * (carController.GetCurrentSpeed() / 2));
        //transform.position = desiredPos;
        LookAt();

    }

    void StandardMovement()
    {
        float distanceCamDes = Mathf.Abs(target.position.z - transform.position.z);
        Vector3 desiredPos = target.position + (target.forward * -distance) + (target.up * height);

        if (distanceCamDes > maxDistance)
        {
            Vector3 tempPos = new Vector3(transform.position.x, transform.position.y, desiredPos.z - (maxDistance - 2));
            //transform.position = Vector3.Lerp(transform.position, tempPos, Time.deltaTime * speed *2);
            transform.position = new Vector3(tempPos.x, tempPos.y, Mathf.Lerp(transform.position.z, tempPos.z, Time.deltaTime * speed * 2));
        }
        //transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * speed);
        transform.position = new Vector3(desiredPos.x, desiredPos.y, Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime * speed));
        Debug.Log("DISTANCE FROM CAR: " + Mathf.Abs(target.position.z - transform.position.z));
        transform.LookAt(target);
    }

    void LookAt()
    {
        var lookPos = target.position - transform.position;

        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;//Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
    }
}
