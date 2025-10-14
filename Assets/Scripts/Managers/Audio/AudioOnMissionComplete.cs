using UnityEngine;

public class AudioOnMissionComplete : MonoBehaviour
{
    [SerializeField] private string missionName = "";
    [SerializeField] private string audioName = "";

    private void Start()
    {
        if (missionName == "") return;
        
        GameManager.gameEventManager.SubscribeToGameEvent(missionName, PlayAudio);
    }

    public void PlayAudio()
    {
        if (audioName == "") return;
        
        // GameManager.audioManager.PlayOneShot(audioName, GameManager.playerController.transform.position);
    }
}
