using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayAudioOnAwake : MonoBehaviour
{
    [SerializeField] private string audioName = "";

    void Awake()
    {
        if (audioName == "") return;
        GameManager.audioManager.PlayOneShot(audioName, transform.position);
    }

}
