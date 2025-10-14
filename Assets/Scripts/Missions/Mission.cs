using System;

[Serializable]
public class Mission
{
    public string title = "";
    public string description = "";

    public bool isCompleted = false;    
    public event Action OnComplete;
    
    public void MissionComplete()
    {
        isCompleted = true;
        OnComplete?.Invoke();
    }
}
