using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LerpToDestination : MonoBehaviour
{
    [SerializeField] private string _missionName;
    
    private Vector3 _startPosition;
    [SerializeField] private Vector3 _localDestination;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private string audioName = "";
    
    private float _totalDuration;
    private bool _isLerping = false;

    protected void Awake()
    {
        _startPosition = transform.localPosition;
    }

    private void Start()
    {
        if (GameManager.missionManager.IsMissionCompleted(_missionName))
            StartLerp();
        else
            GameManager.missionManager.SubscribeToMission(_missionName, StartLerp);
    }
    
    public void StartLerp()
    {
        if (audioName != "")
        {
            GameManager.audioManager.PlayOneShot(audioName, GameManager.playerController.transform.position - Vector3.right * 4.0f);
            Debug.Log("Playing Audio");
        }
            
        _isLerping = true;
    }

    private void FixedUpdate()
    {
        if (_isLerping)
        {
            float interpolate = _totalDuration * _speed;
            if (interpolate > 1.0f) _isLerping = false;
            transform.localPosition = Vector3.Lerp(_startPosition, _localDestination, interpolate);
            _totalDuration += Time.deltaTime;
        }
    }
}
