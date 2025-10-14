using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private string _missionName;
    [SerializeField] private Cutscene cutscene;
    
    private void Start()
    {
        if (GameManager.missionManager.IsMissionCompleted(_missionName))
            cutscene.StartCutscene();
        else
            GameManager.missionManager.SubscribeToMission(_missionName, cutscene.StartCutscene);
    }
}
