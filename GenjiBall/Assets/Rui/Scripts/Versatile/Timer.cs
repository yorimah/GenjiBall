using System.Collections;
using System.Collections.Generic;

public class Timer
{
    public Timer(float _time)
    {
        targetTime = _time;
        time = 0;
    }

    public void resetTime()
    {
        time = 0;
    }

    public void updateTime(float deltaTime)
    {
        time += deltaTime;
        if (time > targetTime) { completeTimer(); }
    }

    public void updateTimeTarget(float time)
    {
        targetTime = time;
    }

    public void completeTimer()
    {
        time = targetTime;
    }

    public float getCurrentTime()
    {
        return time;
    }

    public float getTargetTime()
    {
        return targetTime;
    }

    public bool checkOverTime()
    {
        return time >= targetTime;
    }

    public float getProgress()
    {
        return time / targetTime;
    }

    private float time;
    private float targetTime;
}