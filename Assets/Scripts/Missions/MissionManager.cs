using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private List<Mission> missions = new List<Mission>();
    private Dictionary<string, Mission> missionDict = new Dictionary<string, Mission>();
    public event Action OnAnyMissionComplete;

    private void Awake()
    {
        if (GameManager.missionManager == null)
            GameManager.missionManager = this;
		else 
		{
			Debug.LogError($"A missionManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
    
        for (int i = 0; i < missions.Count; i++)
            AddMission(missions[i]);
    }
    
    public void AddMission(Mission mission)
    {
        missionDict.Add(mission.title, mission);
    }
    
    public void CompleteMission(string missionTitle)
    {
        if (!GetMission(missionTitle, out Mission mission)) return;
        mission.MissionComplete();
    }
    
    public bool IsMissionCompleted(string missionTitle)
    {
        if (!GetMission(missionTitle, out Mission mission)) return false;
        return mission.isCompleted;
    }
    
    public bool GetMission(string missionTitle, out Mission mission)
    {
        if (missionDict.TryGetValue(missionTitle, out mission)) return true;
        
        Debug.Log($"Mission with name: {missionTitle} not found");
        return false;
    }
    
    public void SubscribeToMission(string missionTitle, Action callback)
    {
        if (!GetMission(missionTitle, out Mission mission)) return;
        mission.OnComplete += callback;
    }
    
    public void UnsubscribeToMission(string missionTitle, Action callback)
    {
        if (!GetMission(missionTitle, out Mission mission)) return;
        mission.OnComplete -= callback;
    }
}
