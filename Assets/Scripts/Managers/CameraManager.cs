using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
	[NonSerialized] public Camera mainCam;
	[NonSerialized] public Camera photoCamera;
	public CinemachineCamera cinemachineMainCam;
	public CinemachineCamera cinemachinePhotoCamera;
	[NonSerialized] public Vector2 mainCameraDimensions;
	[NonSerialized] public Vector2 photoCameraDimensions;
	
	[SerializeField] private LayerMask _transparentEnvironmentLayer;
	[SerializeField] public Shader transparentLitShader;
	
	private void Awake()
	{
		if (GameManager.cameraManager == null)
			GameManager.cameraManager = this;
		else 
		{
			Debug.LogError($"A CameraManager already exists, deleting self: {name}");
			Destroy(gameObject);
		}
		
		mainCam = Camera.main;
		
		photoCamera = cinemachinePhotoCamera.GetComponentInChildren<Camera>();
		
		photoCamera.gameObject.SetActive(false);
		
		mainCameraDimensions = GetCameraDimensions(mainCam);
		photoCameraDimensions = GetCameraDimensions(photoCamera);
		
		mainCam.depthTextureMode = DepthTextureMode.Depth;
		
		// Cursor.visible = false;
	}

	public Vector2 GetCameraDimensions(Camera camera )
	{
		float cameraHeight = camera.orthographicSize * 2.0f;
		float cameraWidth = cameraHeight * camera.aspect;
		
		return new Vector2(cameraWidth, cameraHeight);
	}

	public void ActivatePhotoCamera()
	{
		cinemachinePhotoCamera.Priority = 1;
		cinemachineMainCam.Priority = 0;
	    photoCamera.gameObject.SetActive(true);
	}
	
	public void DeactivatePhotoCamera()
	{
		cinemachineMainCam.Priority = 1;
		cinemachinePhotoCamera.Priority = 0;
	    photoCamera.gameObject.SetActive(false);
	}

    // Have transparentEnvironment colliders turn transparent when in between the player and camera 
    private void FixedUpdate()
    {
		PlayerController player = GameManager.playerController;
        float distance = (player.transform.position - mainCam.transform.position).magnitude - 0.5f;
        
        Collider[] collidersCamera = Physics.OverlapCapsule(mainCam.transform.position, mainCam.transform.position + mainCam.transform.forward * distance, 0.4f, _transparentEnvironmentLayer);
        CheckCollidersForTransparency(collidersCamera);
    }
    
    private void CheckCollidersForTransparency(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
			SeeThrough seeThroughObject;
        
			if (colliders[i].attachedRigidbody is not null) 
			{
			    if (!colliders[i].attachedRigidbody.TryGetComponent(out seeThroughObject)) continue;
				seeThroughObject.fading = true;
			}
			else if (colliders[i].TryGetComponent(out seeThroughObject))
			{
				seeThroughObject.fading = true;
			}
			else
				continue;
			
			for (int j = 0; j < seeThroughObject.additionalObjects.Count; j++)
			{
				seeThroughObject.additionalObjects[j].fading = true;
			}
        }
    }
}
