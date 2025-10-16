using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    public string title = "";
    public string description = "";

    public bool isTriggered = false;    
    public UnityEvent OnTrigger;
    
    public void Trigger()
    {
        isTriggered = true;
        OnTrigger?.Invoke();
    }
}