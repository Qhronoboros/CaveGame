using System;
using UnityEngine;

public class Timer
{
    public bool looping = false;
    public bool active = false;
    
    public float duration = 0.0f;
    public float elapsedTime;
    
	public event Action OnTimerStart;
	public event Action OnTimerEnd;
	public event Action OnTimerStopped;
	
	public Timer(float duration = 0.0f)
	{
	    this.duration = duration;
	}
	
    public void StartTimer()
    {
        if (active)
            StopTimer();

        OnTimerStart?.Invoke();
        active = true;
    }
    
    public void StopTimer()
    {
        ResetTimerValues();
        OnTimerStopped?.Invoke();
    }
    
    private void ResetTimerValues()
    {
        active = false;
        elapsedTime = 0.0f;
    }
    
    public void CountTimer(float deltaTime)
    {
        if (!active) return;
        
        elapsedTime += deltaTime;
        
        if (elapsedTime >= duration)
        {
            ResetTimerValues();
            OnTimerEnd?.Invoke();

            if (looping)
                StartTimer();
        }
    }
}
