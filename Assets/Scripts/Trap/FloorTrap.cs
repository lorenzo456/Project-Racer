using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrap : Trap
{
    protected override void Update()
    {
        base.Update();
        TempActivation();

    }

    void TempActivation()
    {
        if (activated)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
            return;

        if (other.CompareTag("CarCollider"))
        {
            other.GetComponent<PlayerCollider>().DisableCar(true, disableTime);
            Debug.Log("DISABLING CAR!");
        }
    }
}
