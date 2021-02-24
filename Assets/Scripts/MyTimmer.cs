using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimmer
{
    public enum STATE
    {
        Idle,
        Run,
        Finished
    }

    public STATE state = STATE.Idle;
    private float duration;
    private float elpasedTime;
    public void Tick()
    {
        switch (state)
        {
            case STATE.Idle:
                break;
            case STATE.Run:
                elpasedTime += Time.deltaTime;
                if (elpasedTime > duration)
                    state = STATE.Finished;
                break;
            case STATE.Finished:
                break;
        }
    }
    public void Go(float _duration)
    {
        duration = _duration;
        elpasedTime = 0;
        state = STATE.Run;
    }
}
