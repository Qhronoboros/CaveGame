using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColor : MonoBehaviour
{
    private Light _light;
    [SerializeField] private string _missionName = "";
    [SerializeField] private Color _color;
    [SerializeField] private float _intensity;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        if (_missionName == "") return;
        
        GameManager.missionManager.SubscribeToMission(_missionName, ChangeColor);
    }
    
    public void ChangeColor()
    {
        _light.color = _color;
        _light.intensity = _intensity;
    }
}
