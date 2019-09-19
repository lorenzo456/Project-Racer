using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnum : MonoBehaviour
{
    public enum ButtonState { ButtonIdle, ButtonPressed, ButtonDown, ButtonUp}

    public ButtonState buttonState = ButtonState.ButtonIdle;

}
