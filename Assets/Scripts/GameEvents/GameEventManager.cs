using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    private Dictionary<string, GameEvent> gameEventDict = new Dictionary<string, GameEvent>();

    public UnityEvent OnGameEventTriggered;

    private void Awake()
    {
        if (GameManager.gameEventManager == null)
            GameManager.gameEventManager = this;
        else
        {
            Debug.LogError($"A GameEventManager already exists, deleting self: {name}");
            Destroy(gameObject);
        }

        for (int i = 0; i < transform.childCount; i++)
            AddGameEvent(transform.GetChild(i).GetComponent<GameEvent>());
    }

    public void AddGameEvent(GameEvent gameEvent)
    {
        if (gameEvent is null)
        {
            Debug.LogError("Given gameObject does not have a GameEvent script");
            return;
        }

        gameEventDict.Add(gameEvent.title, gameEvent);
    }
    
    public void TriggerGameEvent(string gameEventTitle)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return;
        gameEvent.Trigger();
        OnGameEventTriggered.Invoke();
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
    
    public void SubscribeToGameEvent(string gameEventTitle, UnityAction callback)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return;
        gameEvent.OnTrigger.AddListener(callback);
    }
    
    public void UnsubscribeToGameEvent(string gameEventTitle, UnityAction callback)
    {
        if (!GetGameEvent(gameEventTitle, out GameEvent gameEvent)) return;
        gameEvent.OnTrigger.RemoveListener(callback);
    }
}
