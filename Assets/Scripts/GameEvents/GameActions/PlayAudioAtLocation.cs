using UnityEngine;

public class PlayAudioAtLocation : MonoBehaviour
{
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private string audioName = "";

    public void PlayAudio()
    {
        if (!gameObject.activeSelf) return;
        if (audioName == "") return;

        GameManager.audioManager.PlayOneShot(audioName, playPosition);
    }
}