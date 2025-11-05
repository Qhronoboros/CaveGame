using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CapPitchRotation : MonoBehaviour
{
    [SerializeField] private float maxPitch;

    private void FixedUpdate()
    {
        float currentPitch = transform.localEulerAngles.x;
        
        if (currentPitch > 180.0f)
            currentPitch -= 360;

        float clampedPitch = Mathf.Clamp(currentPitch, -maxPitch, maxPitch);
        transform.localEulerAngles = new Vector3(clampedPitch, 0.0f, 0.0f);
    }
}