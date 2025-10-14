using System;

[Serializable]
public class GameEvent
{
    public string title = "";
    public string description = "";

    public bool isTriggered = false;    
    public event Action OnTrigger;
    
    public void Trigger()
    {
        isTriggered = true;
        OnTrigger?.Invoke();
    }
}
