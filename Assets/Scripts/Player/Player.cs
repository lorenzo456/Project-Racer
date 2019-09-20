using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("PLAYER DETAILS")]
    public int playerIndex;

    [Header("TrapInfo")]
    public Trap selectedTrap;

    [Header("GUI")]
    public Text lapText;

    [Header("Controllers")]
    public CarController carController;
    public PlayerCollider playerCollider;
    public WaypointManager waypointManager;

    [Header("Stabalizers")]
    public GameObject carNormal;
    public GameObject backOfCar;

    [Header("TrackInfo")]
    public int trackCount = 0;

    public GameObject camLookAt;

    [SerializeField]
    private int currentSpawnIndex = 0;

    [Header("Inputs")]
    [SerializeField] private bool actionButtonPressed;

    [Header("Controls")]
    public ButtonEnum.ButtonState yButton;
    public ButtonEnum.ButtonState aButton;
    public ButtonEnum.ButtonState backButton;
    public float horizontalAxis;
    public float verticalAxis;
    public float leftTrigger;
    public float rightTrigger;

    public void Start()
    {
        waypointManager = GameObject.FindGameObjectWithTag("WaypointManager").GetComponent<WaypointManager>(); 
    }

    private void Update()
    {
        ControllerInputCheck();
        CheckCarNormal();

        if (actionButtonPressed && selectedTrap != null)
        {
            selectedTrap.Activate();
        }
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }
    public void ControllerInputCheck()
    {
        if (Input.GetButtonDown("ActionButton"))
        {
            actionButtonPressed = true;
        }
        else
        {
            actionButtonPressed = false;
        }
    }

    public Vector3 GetCarPosition()
    {
        return new Vector3(playerCollider.transform.position.x, 0, playerCollider.transform.position.z);
    }

    public int GetCurrentSpawnIndex()
    {
        return currentSpawnIndex;
    }

    public void addCurrentSpawnIndex(int nextSpawnIndex)
    {
        if (currentSpawnIndex + 1 == nextSpawnIndex)
        {
            currentSpawnIndex++;
        }
        else if (nextSpawnIndex == 0 && currentSpawnIndex == waypointManager.GetWayPointCount())
        {
            currentSpawnIndex = 0;
            trackCount++;
            //lapText.text = "LAP: " + trackCount.ToString();
        }
    }

    public void ResetCarPosition()
    {
        if(waypointManager != null)
        {
            playerCollider.transform.parent = null;
            playerCollider.transform.position = waypointManager.waypoints[currentSpawnIndex].GetSpawnPosition();
            // playerCollider.transform.forward = playerCollider.transform.rotation * waypointManager.waypoints[currentSpawnIndex].GetSpawnRotation();
            playerCollider.transform.parent = transform;
            carController.transform.forward = waypointManager.waypoints[currentSpawnIndex].GetSpawnRotation();
        }

        carController.ResetCar();
    }
    public float normalOffset = 0;

    public void CheckCarNormal()
    {

        float backWheelsPosition = (carController.backWheelL.transform.position.z + carController.backWheelR.transform.position.z) * .5f;
        RaycastHit hit;
        
        if(Physics.Raycast(backOfCar.transform.position, carNormal.transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
        {
            Debug.Log("hitting " + hit.transform.name);
        }

        Debug.DrawRay(backOfCar.transform.position, carNormal.transform.TransformDirection(-Vector3.up) * hit.distance, Color.white);

            //carNormal.transform.eulerAngles = Vector3.Lerp(carNormal.transform.eulerAngles, hit.transform.position, Time.deltaTime * 2f);
        
    }
}
