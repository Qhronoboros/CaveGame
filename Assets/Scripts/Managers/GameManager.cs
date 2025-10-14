using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set;}
	// public static PlayerController playerController;
	public static UIManager uiManager;
	// public static PauseMenu pauseMenu;
	public static AudioManager audioManager;
	public static GameEventManager gameEventManager;
	public static SerialCommunication serialCommunication;
	
	public bool gameHasEnded = false;
	
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
}
