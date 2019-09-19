using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ScreenManager screenManager;
    public List<Player> players = new List<Player>();
    public List<Transform> trackPositions = new List<Transform>();

    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject cameraList;

    public static GameManager instance;
    public Camera mainCamera;

    public bool CheckIfPlayerDuplicate(int index)
    {
        foreach(Player p in players)
        {
            if(p.playerIndex == index)
            {
                return true;
            }
        }
        return false;
    }

    public void AddPlayer(int index)
    {
        Debug.Log("ADDED PLAYER");
        GameObject tempPlayer = Instantiate(playerPrefab);
        tempPlayer.transform.position = trackPositions[index].transform.position;
        Player p = tempPlayer.GetComponent<Player>();
        p.SetPlayerIndex(index);
        GameObject tempCam = Instantiate(cameraPrefab);
        tempCam.transform.parent = cameraList.transform;
        Destroy(tempCam.GetComponent<AudioListener>());
        CamFollow cf = tempCam.GetComponent<CamFollow>();
        cf.target = p.camLookAt.transform;
        cf.carController = p.carController;
        players.Add(p);
        screenManager.UpdateScreenRects();
    }
    public void SetButtonInput(int index, ButtonEnum.ButtonState pressedA = ButtonEnum.ButtonState.ButtonIdle, ButtonEnum.ButtonState pressedB = ButtonEnum.ButtonState.ButtonIdle, ButtonEnum.ButtonState pressedX = ButtonEnum.ButtonState.ButtonIdle, ButtonEnum.ButtonState pressedY = ButtonEnum.ButtonState.ButtonIdle)
    {
        players[index].aButton = pressedA;
        players[index].yButton = pressedY;
    }

    public void SetAxisInput(int index, float verticalAxis = 0f, float horizontalAxis = 0f)
    {
        players[index].verticalAxis = verticalAxis;
        players[index].horizontalAxis = horizontalAxis;
    }

    public void SetTriggers(int index, float leftTrigger = 0, float rightTrigger = 0)
    {
        players[index].leftTrigger = leftTrigger;
        players[index].rightTrigger = rightTrigger;
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


}
