using System;
using UnityEngine;

public class Timer
{
    private bool _looping = false;
    public bool counting = false;
    
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
        if (counting)
            StopTimer();

        OnTimerStart?.Invoke();
        counting = true;
    }

    public void EnableLooping()
    {
        if (!_looping)
        {
            _looping = true;
            OnTimerEnd += StartTimer;
        }
    }
    
    public void DisableLooping()
    {
        if (_looping)
        {
            _looping = false;
            OnTimerEnd -= StartTimer;
        }
    }
    
    public void StopTimer()
    {
        ResetTimerValues();
        OnTimerStopped?.Invoke();
    }
    
    private void ResetTimerValues()
    {
        counting = false;
        elapsedTime = 0.0f;
    }
    
    public void CountTimer(float deltaTime)
    {
        if (!counting) return;
        
        elapsedTime += deltaTime;
        
        if (elapsedTime >= duration)
        {
            ResetTimerValues();
            OnTimerEnd?.Invoke();
        }
    }
}
