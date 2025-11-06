using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Hands;

public class CrawlingHand
{
    public Crawling crawlingParent;
    public bool isActive;
    public Handedness handedness;

    public float forwardMagnitude;
    public float turnAmount;

    private XRHandJoint _joint;
    public XRHandJoint Joint
    {
        get { return _joint; }
        set
        {
            _joint = value;
            UpdateMovementValues();
        }
    }
    private Vector3 _lastJointLocalCameraPosition;

    public CrawlingHand(Crawling crawlingParent, Handedness handedness)
    {
        isActive = false;
        this.crawlingParent = crawlingParent;
        this.handedness = handedness;
    }

    // Given joint should be on the same hand
    public void SetJoint(XRHandJoint handJoint)
    {
        Joint = handJoint;
        isActive = true;
    }

    public void SetInactive()
    {
        Joint = default;
        isActive = false;
        _lastJointLocalCameraPosition = Vector2.zero;
        forwardMagnitude = 0.0f;
        turnAmount = 0.0f;
    }

    public void UpdateMovementValues()
    {
        if (!isActive) return;

        Joint.TryGetPose(out Pose jointPose);
        if (jointPose == null) return;

        Pose jointPoseWS = jointPose.GetTransformedBy(
            new Pose(crawlingParent.origin.transform.position, crawlingParent.origin.transform.rotation));

        // * No rotations, since it would mess up comparing with older data
        // XROrigin to World Space
        // Vector3 jointPosePositionWS = jointPose.position + crawlingParent.origin.transform.position;

        // World Space to Camera
        // Vector3 jointLocalCameraPosition = jointPosePositionWS - Camera.main.transform.position;
        Vector3 jointLocalCameraPosition = jointPoseWS.position - Camera.main.transform.position;

        if (_lastJointLocalCameraPosition == Vector3.zero)
        {
            _lastJointLocalCameraPosition = jointLocalCameraPosition;
            return;
        }

        Vector2 jointLocalCameraPositionXZ = VectorHelper.Vector3ToVector2(jointLocalCameraPosition);
        Vector2 _lastJointLocalCameraPositionXZ = VectorHelper.Vector3ToVector2(_lastJointLocalCameraPosition);

        // Change the turn amount depending on how close the joint is to the camera
        float turnMultiplier = Mathf.Min(jointLocalCameraPositionXZ.magnitude * 1.0f, 1.0f);
        turnAmount = Vector2.SignedAngle(_lastJointLocalCameraPositionXZ, jointLocalCameraPositionXZ) * turnMultiplier;

        float lastJointPositionMagnitude = _lastJointLocalCameraPositionXZ.magnitude;
        float jointPositionMagnitude = jointLocalCameraPositionXZ.magnitude;

        // forwardMagnitude = Mathf.Max(lastJointPositionMagnitude - jointPositionMagnitude, 0.0f);
        forwardMagnitude = lastJointPositionMagnitude - jointPositionMagnitude;

        _lastJointLocalCameraPosition = jointLocalCameraPosition;

        if (handedness == Handedness.Right)
        {
            // Debug stuff
            // Debug.Log($"ForwardMagnitude: {forwardMagnitude}");
            // Debug.Log($"TurnMultiplier: {turnMultiplier}");
        }

        // GameManager.changeDebugText.ChangeText($"{magnitudeDelta}");
    }
}