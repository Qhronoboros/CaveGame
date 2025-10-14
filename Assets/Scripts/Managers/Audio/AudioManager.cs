using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(MusicPlayer))]
public class AudioManager : MonoBehaviour
{
	public MusicPlayer musicPlayer;
	[SerializeField] private AudioDataSO _audioData;
	private Dictionary<string, EventReference> _audioEventsDict = new Dictionary<string, EventReference>(); // Contains all eventReferences
	private Dictionary<string, EventInstance> _audioInstancesDict = new Dictionary<string, EventInstance>(); // Contains pre instantiated eventInstances for specific sounds/music/ambience
	private List<EventInstance> _eventInstanceList = new List<EventInstance>(); // List for stopping and destroying on deletion
	private List<StudioEventEmitter> _eventEmitterList = new List<StudioEventEmitter>(); // List for stopping on deletion

	// Temporary
	[SerializeField] private bool _isInMainMenu;

	private void Awake() 
	{
		if (GameManager.audioManager == null) { GameManager.audioManager = this; }
		else 
		{
			Debug.LogError($"An AudioManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
		
		musicPlayer = GetComponent<MusicPlayer>();
		
		// Fill the AudioEventsDict and fill the audioInstancesDict with instantiated audio EventInstances
		// Also fill the MusicInstancesDict with insantiated music EventInstances;
		for (int i = 0; i < _audioData.audioLinkList.Count; i++)
		{
			AudioDataSO.AudioLink audioLink = _audioData.audioLinkList[i];
			_audioEventsDict.Add(audioLink.audioName, audioLink.eventReference);
			
			// If not preInsantiate and isMusic
			if (!audioLink.preInstantiate && !audioLink.isMusic) continue;
			
			CreateEventInstance(audioLink.audioName, out EventInstance instance);
			
			// fill the audioInstancesDict with pre instantiated audio EventInstance
			_audioInstancesDict.Add(audioLink.audioName, instance);
				
			// If isMusic, fill the musicInstancesDict with pre instantiated audio EventInstance
			if (audioLink.isMusic)
				musicPlayer.musicInstancesDict.Add(audioLink.audioName, instance);
		}
		
		// Start Ambient sounds
		// PlayInstanceAtLocation("OceanWaves", gameObject);
		// PlayInstanceAtLocation("Wind", gameObject);
		// // 0 is safe room, 1 is default, 2 is picking up photo, 3 is getting chased
		// SetEventInstanceParameter("Heartbeat", "Heartbeat Frequency", 1);
		
		if (_isInMainMenu)
		{
			PlayInstanceAtLocation("BarMusic", gameObject);
		}
	}
	
	// Plays the audio once at location
	public void PlayOneShot(string audioName, Vector3 location)
	{
		if (FindEventReference(audioName, out EventReference eventReference)) {RuntimeManager.PlayOneShot(eventReference, location);}
	}
	
	// Plays the audio once at gameObject (Follows the gameObject)
	public void PlayOneShotAttached(string audioName, GameObject gameObject)
	{
		if (FindEventReference(audioName, out EventReference eventReference)) {RuntimeManager.PlayOneShotAttached(eventReference, gameObject);}
	}
	
	// Detaches all gameObjects from the eventInstance, attach the new given gameObject to the eventInstance, and play the audio
	public void PlayInstanceAtLocation(EventInstance eventInstance, GameObject gameObject)
	{
		RuntimeManager.DetachInstanceFromGameObject(eventInstance);
		RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject);
		eventInstance.start();
	}
	
	// Detaches all gameObjects from the eventInstance with given audioName, attach the new given gameObject to the eventInstance, and play the audio
	// (Given audio should be instantiated on awake)
	public void PlayInstanceAtLocation(string audioName, GameObject gameObject)
	{
		if (_audioInstancesDict.TryGetValue(audioName, out EventInstance eventInstance))
		{
			PlayInstanceAtLocation(eventInstance, gameObject);
		}
		else { Debug.LogError($"Could not find eventInstance in _audioInstancesDict with audioName: {audioName}"); }
	}
	
	// Stops the given eventInstance from playing
	public void StopPlayingInstance(EventInstance eventInstance)
	{
		eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
	
	// Stops the given eventInstance with given audioName from playing
	public void StopPlayingInstance(string audioName)
	{
		if (_audioInstancesDict.TryGetValue(audioName, out EventInstance eventInstance))
		{
			StopPlayingInstance(eventInstance);
		}
		else { Debug.LogError($"Could not find eventInstance in _audioInstancesDict with audioName: {audioName}"); }
	}
	
	// Creates an eventInstance with the given audioName, returns it by using the out keyword
	// And return if it was successful or not with a boolean
	public bool CreateEventInstance(string audioName, out EventInstance eventInstance)
	{
		bool referenceFound = FindEventReference(audioName, out EventReference eventReference);
		eventInstance = RuntimeManager.CreateInstance(eventReference);
		
		if (referenceFound) { _eventInstanceList.Add(eventInstance); }
		
		return referenceFound;
	}
	
	// Sets the value for a parameter of the given eventInstance
	public void SetEventInstanceParameter(EventInstance eventInstance, string parameterName, float parameterValue)
	{
		eventInstance.setParameterByName(parameterName, parameterValue);
	}
	
	// Sets the value for a parameter of the eventInstance with the given audioName
	public void SetEventInstanceParameter(string audioName, string parameterName, float parameterValue)
	{
		if (_audioInstancesDict.TryGetValue(audioName, out EventInstance eventInstance))
		{
			SetEventInstanceParameter(eventInstance, parameterName, parameterValue);
		}
		else { Debug.LogError($"Could not find eventInstance in _audioInstancesDict with audioName: {audioName}"); }
	}
	
	// Initializes the given eventEmitter from the GameObject with the given audioName, returns it by using the out keyword
	// And return if it was successful or not with a boolean
	public bool InitializeEventEmitter(string audioName, GameObject gameObject, out StudioEventEmitter emitter)
	{
		bool referenceFound = FindEventReference(audioName, out EventReference eventReference);
		emitter = gameObject.GetComponent<StudioEventEmitter>();
		
		if (referenceFound)
		{
			emitter.EventReference = eventReference;
			_eventEmitterList.Add(emitter);
		}
		return referenceFound;
	}
	
	// Tries to find an eventReference in the _audioEventsDict with the given audioName, returns it by using the out keyword
	// And return if it was successful or not with a boolean
	private bool FindEventReference(string audioName, out EventReference eventReference)
	{
		if (_audioEventsDict.TryGetValue(audioName, out eventReference)) { return true; }
		else { Debug.LogError($"Could not find eventReference with audioName: {audioName}"); return false; }
	}
	
	// Stop and release all the eventInstances and emitters in their lists
	private void CleanUp()
	{
		for (int i = 0; i < _eventInstanceList.Count; i++)
		{
			_eventInstanceList[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			_eventInstanceList[i].release();
		}
		for (int i = 0; i < _eventEmitterList.Count; i++)
		{
			_eventEmitterList[i].Stop();
		}
	}
	
	private void OnDestroy()
	{
		CleanUp();
	}
}
