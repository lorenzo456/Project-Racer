using UnityEngine;
using XInputDotNetPure; 

public class ControllerManager : MonoBehaviour
{

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    int controllerIndex;
    bool anyControllerFound = false;

    void FixedUpdate()
    {
        PlayerIndex temp = (PlayerIndex)1;
        GamePad.SetVibration(temp, state.Triggers.Left, state.Triggers.Right);
        PlayerIndex temp2 = (PlayerIndex)0;
        GamePad.SetVibration(temp2, state.Triggers.Left, state.Triggers.Right);
    }


    // Update is called once per frame
    void Update()
    {
        int controllersFound = 0;
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    if (!GameManager.instance.CheckIfPlayerDuplicate(i))
                    {
                        GameManager.instance.AddPlayer(i);
                    }
                    playerIndexSet = true;
                    controllersFound++;
                }
            }
            if(controllersFound > 0)
            {
                anyControllerFound = true;
            }
            else
            {
                anyControllerFound = false;
            }
        }

        for(int i = 0; i < controllersFound; i++)
        {
            PlayerIndex tempplayerIndex = (PlayerIndex)i;
            GamePadState tempstate = GamePad.GetState(tempplayerIndex);

            CheckInput(tempplayerIndex, tempstate);

        }
        /*
        if (playerIndex.ToString() == "One")
        {
            //Debug.Log("ONEEE");
            //controllerIndex = 0;
            //PlayerIndex testPlayerIndex = (PlayerIndex)0;
           // CheckInput(0, testPlayerIndex);

        }
        if (playerIndex.ToString() == "Two")
        {
            //controllerIndex = 1;
            //PlayerIndex testPlayerIndex = (PlayerIndex)1;
            //CheckInput(1, testPlayerIndex);
        }
        
        prevState = state;
        state = GamePad.GetState(playerIndex);
        
        ButtonEnum.ButtonState a = ButtonEnum.ButtonState.ButtonIdle;
        ButtonEnum.ButtonState b = ButtonEnum.ButtonState.ButtonIdle;
        ButtonEnum.ButtonState x = ButtonEnum.ButtonState.ButtonIdle;
        ButtonEnum.ButtonState y = ButtonEnum.ButtonState.ButtonIdle;
       
        
        if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Pressed)
        {
            a = ButtonEnum.ButtonState.ButtonPressed;
            Debug.Log("PRESSED A  PLAYER: " + playerIndex );
        }
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            //Debug.Log("button A is pressed ");
            a = ButtonEnum.ButtonState.ButtonDown;
        }
        if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
        {
            //Debug.Log("button A is released");
            a = ButtonEnum.ButtonState.ButtonUp;
        }


        if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Pressed)
        {
            y = ButtonEnum.ButtonState.ButtonPressed;
        }
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            //Debug.Log("button A is pressed ");
            y = ButtonEnum.ButtonState.ButtonDown;
        }
        if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
        {
            //Debug.Log("button A is released");
            y = ButtonEnum.ButtonState.ButtonUp;
        }

        if (state.Triggers.Right > 0)
        {
            Debug.Log("RIGHT TRIGGER: " + state.Triggers.Right);
        }

        if (state.Triggers.Left > 0)
        {
            Debug.Log("LEFT TRIGGER: " + state.Triggers.Left);
        }

        if (state.ThumbSticks.Left.X != 0)
        {
            Debug.Log("HORIZONTAL: " + state.ThumbSticks.Left.X);
        }

        if (state.ThumbSticks.Left.Y != 0)
        {
            Debug.Log("VERTICAL: " + state.ThumbSticks.Left.Y);
        }

        if (anyControllerFound)
        {
            GameManager.instance.SetButtonInput((int)playerIndex, a, b, x, y);
            GameManager.instance.SetAxisInput((int)playerIndex, state.ThumbSticks.Left.Y, state.ThumbSticks.Left.X);
            GameManager.instance.SetTriggers((int)playerIndex, state.Triggers.Left, state.Triggers.Right);

        }
        */
    }
    
    public void CheckInput(PlayerIndex tempPlayerIndex, GamePadState tempCurrentState)
    {

        GamePadState tempPrevState = tempCurrentState;
        tempCurrentState = GamePad.GetState(tempPlayerIndex);

        ButtonEnum.ButtonState a = ButtonEnum.ButtonState.ButtonIdle;
        ButtonEnum.ButtonState b = ButtonEnum.ButtonState.ButtonIdle;
        ButtonEnum.ButtonState x = ButtonEnum.ButtonState.ButtonIdle;
        ButtonEnum.ButtonState y = ButtonEnum.ButtonState.ButtonIdle;


        if (tempPrevState.Buttons.A == ButtonState.Pressed && tempCurrentState.Buttons.A == ButtonState.Pressed)
        {
            a = ButtonEnum.ButtonState.ButtonPressed;
        }
        if (tempPrevState.Buttons.A == ButtonState.Released && tempCurrentState.Buttons.A == ButtonState.Pressed)
        {
            //Debug.Log("button A is pressed ");
            a = ButtonEnum.ButtonState.ButtonDown;
        }
        if (tempPrevState.Buttons.A == ButtonState.Pressed && tempCurrentState.Buttons.A == ButtonState.Released)
        {
            //Debug.Log("button A is released");
            a = ButtonEnum.ButtonState.ButtonUp;
        }


        if (prevState.Buttons.Y == ButtonState.Pressed && tempCurrentState.Buttons.Y == ButtonState.Pressed)
        {
            y = ButtonEnum.ButtonState.ButtonPressed;
        }
        if (tempPrevState.Buttons.Y == ButtonState.Released && tempCurrentState.Buttons.Y == ButtonState.Pressed)
        {
            //Debug.Log("button A is pressed ");
            y = ButtonEnum.ButtonState.ButtonDown;
        }
        if (tempPrevState.Buttons.Y == ButtonState.Pressed && tempCurrentState.Buttons.Y == ButtonState.Released)
        {
            //Debug.Log("button A is released");
            y = ButtonEnum.ButtonState.ButtonUp;
        }

        if (tempCurrentState.Triggers.Right > 0)
        {
            Debug.Log("RIGHT TRIGGER: " + tempCurrentState.Triggers.Right);
        }

        if (tempCurrentState.Triggers.Left > 0)
        {
            Debug.Log("LEFT TRIGGER: " + tempCurrentState.Triggers.Left);
        }

        if (tempCurrentState.ThumbSticks.Left.X != 0)
        {
            Debug.Log("HORIZONTAL: " + tempCurrentState.ThumbSticks.Left.X);
        }

        if (tempCurrentState.ThumbSticks.Left.Y != 0)
        {
            Debug.Log("VERTICAL: " + tempCurrentState.ThumbSticks.Left.Y);
        }

        if (anyControllerFound)
        {
            GameManager.instance.SetButtonInput((int)tempPlayerIndex, a, b, x, y);
            GameManager.instance.SetAxisInput((int)tempPlayerIndex, tempCurrentState.ThumbSticks.Left.Y, tempCurrentState.ThumbSticks.Left.X);
            GameManager.instance.SetTriggers((int)tempPlayerIndex, tempCurrentState.Triggers.Left, tempCurrentState.Triggers.Right);

        }
    }
}
