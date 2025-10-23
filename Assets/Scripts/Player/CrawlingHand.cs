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
    private Vector2 _lastJointLocalCameraPosition;

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
        isActive = false;
        _lastJointLocalCameraPosition = Vector2.zero;
        forwardMagnitude = 0.0f;
        turnAmount = 0.0f;
    }

    public void UpdateMovementValues()
    {
        if (!isActive) return;

        Joint.TryGetPose(out Pose jointPose);
        
        // XROrigin to World Space
        Pose jointPoseWS = jointPose.GetTransformedBy(crawlingParent._origin.transform);
        Vector2 jointPosePositionWS = new Vector2(jointPoseWS.position.x, jointPoseWS.position.z);

        // World Space to Camera
        Vector2 cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);
        Vector2 jointLocalCameraPosition = jointPosePositionWS - cameraPosition;

        if (_lastJointLocalCameraPosition == Vector2.zero)
        {
            _lastJointLocalCameraPosition = jointLocalCameraPosition;
            return;
        }

        // Change the turn amount depending on how close the hand is to the camera
        turnAmount = Vector2.SignedAngle(_lastJointLocalCameraPosition, jointLocalCameraPosition);

        float lastJointPositionMagnitude = _lastJointLocalCameraPosition.magnitude;
        float jointPositionMagnitude = jointLocalCameraPosition.magnitude;

        forwardMagnitude = (lastJointPositionMagnitude - jointPositionMagnitude) * 1.0f;

        _lastJointLocalCameraPosition = jointLocalCameraPosition;

        // GameManager.changeDebugText.ChangeText($"{magnitudeDelta}");
    }
}