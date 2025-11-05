using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CapPitchRotation : MonoBehaviour
{
    [SerializeField] private float maxPitch;

    // ! This is ugly, don't look
    private void FixedUpdate()
    {
        // Vector3 forward2D = new Vector3(gameObject.transform.forward.x, 0.0f, gameObject.transform.forward.z);
        // Vector3 forward2DNormalized = forward2D.normalized;

        // float signedAngle = Vector3.SignedAngle(forward2DNormalized, gameObject.transform.forward, Vector3.up);

        // if (signedAngle > )

        // float currentX = RotationHelper.ClampDegrees(transform.localEulerAngles.x);
        // float minX = RotationHelper.ClampDegrees(-maxPitch);
        // float maxX = RotationHelper.ClampDegrees(maxPitch);

        // float outValue = transform.localEulerAngles.x;
        // if (RotationHelper.ClampDegrees(currentX - maxPitch) < minX) outValue = -maxPitch;
        // else if (RotationHelper.ClampDegrees(currentX + maxPitch) > maxX) outValue = maxPitch;

        float minimumOffset = (-maxPitch + 360) % 360;
        float maximumOffset = (maxPitch + 360) % 360;
        float currentOffset = (transform.localEulerAngles.x + 360) % 360;

        float outValue = Mathf.Clamp(currentOffset, minimumOffset, maximumOffset);
    
        transform.localEulerAngles = new Vector3(outValue - 360, 0.0f, 0.0f);
    }
}