using UnityEngine;

public class ControlAudioInstance : MonoBehaviour
{
    [SerializeField] private GameObject _attachObject;
    [SerializeField] private string _audioName = "";

    public void PlayAudio()
    {
        if (!gameObject.activeSelf) return;
        if (_audioName == "" || _attachObject == null) return;

        GameManager.audioManager.PlayInstanceAttached(_audioName, _attachObject);
    }

    public void StopAudio()
    {
        if (!gameObject.activeSelf) return;
        if (_audioName == "" || _attachObject == null) return;

        GameManager.audioManager.StopPlayingInstance(_audioName);
    }
}