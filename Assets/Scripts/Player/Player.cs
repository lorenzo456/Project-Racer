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
    public float horizontalAxis;
    public float verticalAxis;
    public float leftTrigger;
    public float rightTrigger;

    private void Update()
    {
        ControllerInputCheck();

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
            lapText.text = "LAP: " + trackCount.ToString();
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
}
