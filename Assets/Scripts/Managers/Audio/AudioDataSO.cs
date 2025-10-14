using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu]
public class AudioDataSO : ScriptableObject
{
	[System.Serializable]
	public struct AudioLink
	{
		public string audioName; // The name of the audio
		public EventReference eventReference; // Audio Location
		public bool preInstantiate; // Set to true if audio should be able to be modified
		public bool isMusic; // If the audio is music, preInsantiate automatically becomes true if isMusic is true
	}
	
	public List<AudioLink> audioLinkList = new List<AudioLink>();
}
