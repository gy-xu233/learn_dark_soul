using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool onPressing;
    public bool isPressed;
    public bool isReleased;
    public bool isExtending;

    private bool currentState;
    private bool lastState;
    private float timmerDuration = 0.15f;
    private MyTimmer timmer = new MyTimmer();

    public void Tick(bool input)
    {
        timmer.Tick();
        currentState = input;
        onPressing = currentState;
        isPressed = currentState && (!lastState);
        isReleased = !currentState && lastState;
        if(isReleased)
        {
            timmer.Go(timmerDuration);
        }
        isExtending = (timmer.state == MyTimmer.STATE.Run);
        lastState = currentState;
    }
}
