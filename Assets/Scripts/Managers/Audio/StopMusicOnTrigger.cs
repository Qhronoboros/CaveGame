using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusicOnTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // if (!other.isTrigger && other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out PlayerController player))
        // {
            // GameManager.audioManager.musicPlayer.StopMusic();
        // }
    }
    
}
