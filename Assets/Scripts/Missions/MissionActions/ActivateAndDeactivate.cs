using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAndDeactivate : MonoBehaviour
{
    [SerializeField] private string _missionName;
    
    [SerializeField] private List<GameObject> _SetActiveGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> _SetInactiveGameObjects = new List<GameObject>();

    private void Start()
    {
        if (GameManager.missionManager.IsMissionCompleted(_missionName))
            SetActivityGameObjects();
        else 
            GameManager.missionManager.SubscribeToMission(_missionName, SetActivityGameObjects);
    }
    
    public void SetActivityGameObjects()
    {
        // Debug.Log("Setting Activity");
    
        for (int i = 0; i < _SetActiveGameObjects.Count; i++)
        {
            _SetActiveGameObjects[i].SetActive(true);
        }
        
        for (int i = 0; i < _SetInactiveGameObjects.Count; i++)
        {
            _SetInactiveGameObjects[i].SetActive(false);
        }
    }
}
