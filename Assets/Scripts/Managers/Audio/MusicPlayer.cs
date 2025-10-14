using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
public class MusicPlayer : MonoBehaviour
{
	private AudioManager _audioManager;
	public Dictionary<string, EventInstance> musicInstancesDict = new Dictionary<string, EventInstance>(); // Contains pre instantiated music eventInstances
    public string currentSongName;
    public EventInstance currentSong;

    private void Awake()
    {
        _audioManager = GetComponent<AudioManager>();
    }

    public void PlayMusic(string songName)
    {
        if (currentSongName == songName) return;
        
        if (GetPlayBackState() != PLAYBACK_STATE.STOPPED)
            currentSong.stop(STOP_MODE.ALLOWFADEOUT);
        
        currentSongName = songName;
        currentSong = GetEventInstance(songName);
        
        _audioManager.PlayInstanceAtLocation(currentSong, GameManager.playerController.gameObject);
    }
    
    public void StopMusic()
    {
        if (GetPlayBackState() != PLAYBACK_STATE.STOPPED)
        {
            _audioManager.StopPlayingInstance(currentSong);
            currentSongName = "";
        }
    }
    
    public void PauseMusic()
    {
        if (!GetPaused() && GetPlayBackState() == PLAYBACK_STATE.PLAYING)
            currentSong.setPaused(true);
    }
    
    public void ResumeMusic()
    {
        if (GetPaused() && GetPlayBackState() == PLAYBACK_STATE.PLAYING)
            currentSong.setPaused(false);
    }

    public PLAYBACK_STATE GetPlayBackState()
    {
        currentSong.getPlaybackState(out PLAYBACK_STATE state);
        return state;
    }
    
    public bool GetPaused()
    {
        currentSong.getPaused(out bool paused);
        return paused;
    }
    
    public EventInstance GetEventInstance(string songName)
    {
        musicInstancesDict.TryGetValue(songName, out EventInstance eventInstance);
        return eventInstance;
    }
}
