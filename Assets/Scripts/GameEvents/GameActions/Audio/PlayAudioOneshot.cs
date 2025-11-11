using UnityEngine;

public class PlayAudioOneshot : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private string _audioName = "";
    [SerializeField] private bool _attach = false;

    public void PlayAudio()
    {
        if (!gameObject.activeSelf) return;
        if (_audioName == "" || _gameObject == null) return;

        if (_attach)
            GameManager.audioManager.PlayOneShotAttached(_audioName, _gameObject);
        else
            GameManager.audioManager.PlayOneShot(_audioName, _gameObject.transform.position);
    }
}