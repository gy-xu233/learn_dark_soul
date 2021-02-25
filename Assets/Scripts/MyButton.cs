using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool onPressing;
    public bool isPressed;
    public bool isReleased;
    public bool isExtending;
    public bool isLagging;

    private bool currentState;
    private bool lastState;
    private float extendingDuration = 0.15f;
    private float lagDuration = 0.15f;

    private MyTimmer extendingTimmer = new MyTimmer();
    private MyTimmer lagTimmer = new MyTimmer();

    public void Tick(bool input)
    {
        extendingTimmer.Tick();
        lagTimmer.Tick();
        currentState = input;
        onPressing = currentState;
        isPressed = currentState && (!lastState);
        isReleased = !currentState && lastState;
        if(isReleased)
        {
            extendingTimmer.Go(extendingDuration);
        }
        if(isPressed)
        {
            lagTimmer.Go(lagDuration);
        }
        isExtending = (extendingTimmer.state == MyTimmer.STATE.Run);
        isLagging = (lagTimmer.state == MyTimmer.STATE.Run);

        lastState = currentState;
    }
}
