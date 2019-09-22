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
    Vector3 originalForward;

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
        originalForward = transform.forward;
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
        if(playerCollider.colliderNormal != new Vector3(0, 1, 0))
        {
            Debug.Log(playerCollider.colliderNormal);
            Quaternion temp = Quaternion.FromToRotation(carNormal.transform.up, playerCollider.colliderNormal) * carNormal.transform.rotation;

            carNormal.transform.rotation = Quaternion.Lerp(carNormal.transform.rotation, temp, Time.deltaTime * 15);
        }
        else
        {
            carNormal.transform.eulerAngles = carController.transform.eulerAngles;
            Quaternion temp = Quaternion.FromToRotation(carNormal.transform.up, carController.transform.up) * carNormal.transform.rotation;

            carNormal.transform.rotation = Quaternion.Lerp(carNormal.transform.rotation, temp, Time.deltaTime * 15);
        }
       
    }

}
