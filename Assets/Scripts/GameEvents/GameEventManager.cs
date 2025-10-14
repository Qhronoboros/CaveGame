using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] private List<GameEvent> gameEvents = new List<GameEvent>();
    private Dictionary<string, GameEvent> gameEventDict = new Dictionary<string, GameEvent>();
    public event Action OnAnyGameEventTriggered;

    private void Awake()
    {
        if (GameManager.gameEventManager == null)
            GameManager.gameEventManager = this;
		else 
		{
			Debug.LogError($"A gameEventManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
    
        for (int i = 0; i < gameEvents.Count; i++)
            AddGameEvent(gameEvents[i]);
    }
    
    public void AddGameEvent(GameEvent gameEvent)
    {
        gameEventDict.Add(gameEvent.title, gameEvent);
    }
    
    public void TriggerGameEvent(string gameEventTitle)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return;
        gameEvent.Trigger();
    }
    
    public bool IsGameEventTriggered(string gameEventTitle)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return false;
        return gameEvent.isTriggered;
    }
    
    public bool GetGameEvent(string gameEventTitle, out GameEvent gameEvent)
    {
        if (gameEventDict.TryGetValue(gameEventTitle, out gameEvent)) return true;
        
        Debug.Log($"GameEvent with name: {gameEventTitle} not found");
        return false;
    }
    
    public void SubscribeToGameEvent(string gameEventTitle, Action callback)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return;
        gameEvent.OnTrigger += callback;
    }
    
    public void UnsubscribeToGameEvent(string gameEventTitle, Action callback)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return;
        gameEvent.OnTrigger -= callback;
    }
}
