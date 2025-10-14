using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set;}
	public static PlayerController playerController;
	public static CameraManager cameraManager;
	public static UIManager uiManager;
	public static PauseMenu pauseMenu;
	public static AudioManager audioManager;
	public static MissionManager missionManager;
	
	public List<PhotoTarget> photoTargets;
	
	public event Action OnCutsceneStart;
	public event Action OnCutsceneEnd;
	
	public event Action OnCutscenePreemptiveStop;
	
	public bool gameHasEnded = false;
	
	public event Action OnPlayerCaught;
	public event Action OnPlayerWin;
	
	// Temporary
	public string Caughtreason = "";
	
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else 
		{
			Debug.LogError($"A GameManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
	}
	
	public void CutsceneStarting()
	{
	    OnCutsceneStart?.Invoke();
	}
	
	public void CutsceneEnding()
	{
	    OnCutsceneEnd?.Invoke();
	}
	
	public void PreemptiveCutsceneStop()
	{
	    OnCutscenePreemptiveStop?.Invoke();
	}
	
	public void PlayerCaught(string reason)
	{
		if (gameHasEnded) return;
		
		gameHasEnded = true;
		Debug.Log("Caught");
		Caughtreason = reason;
		audioManager.PlayOneShot("GameOver", playerController.transform.position);
	    OnPlayerCaught?.Invoke();
	}
	
	public void PlayerWin()
	{
		if (gameHasEnded) return;
		
		gameHasEnded = true;
	    OnPlayerWin?.Invoke();
	}
}
