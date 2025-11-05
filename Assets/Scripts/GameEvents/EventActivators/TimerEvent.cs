using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    public bool startOnAwake = false;
    public bool looping = false;
    public bool active = false;
    
    public float duration = 0.0f;
    public float elapsedTime;
    
	public UnityEvent TimerStart;
	public UnityEvent TimerEnd;
    public UnityEvent TimerStopped;

    void Awake()
    {
        if (startOnAwake)
            StartTimer();
    }

    private void Update()
    {
        if (!active) return;
        
        elapsedTime += Time.deltaTime;
        
        if (elapsedTime >= duration)
        {
            ResetTimerValues();
            TimerEnd?.Invoke();

            if (looping)
                StartTimer();
        }
    }

    public void StartTimer()
    {
        if (active)
            StopTimer();

        TimerStart?.Invoke();
        active = true;
    }
    
    public void StopTimer()
    {
        ResetTimerValues();
        TimerStopped?.Invoke();
    }
    
    private void ResetTimerValues()
    {
        active = false;
        elapsedTime = 0.0f;
    }
}
