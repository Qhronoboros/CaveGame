using UnityEngine;
using UnityEngine.XR.Hands;

public class CrawlingHand
{
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
    private Vector2 _lastJointLocalXROriginPosition;

    public CrawlingHand(Handedness handedness)
    {
        isActive = false;
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
        _lastJointLocalXROriginPosition = Vector2.zero;
        forwardMagnitude = 0.0f;
        turnAmount = 0.0f;
    }

    public void UpdateMovementValues()
    {
        if (!isActive) return;

        Joint.TryGetPose(out Pose jointPose);
        Vector2 jointLocalXROriginPosition = new Vector2(jointPose.position.x, jointPose.position.z);

        if (_lastJointLocalXROriginPosition == Vector2.zero)
        {
            _lastJointLocalXROriginPosition = jointLocalXROriginPosition;
            return;
        }

        turnAmount = Vector2.SignedAngle(_lastJointLocalXROriginPosition, jointLocalXROriginPosition);

        float lastJointPositionMagnitude = _lastJointLocalXROriginPosition.magnitude;
        float jointPositionMagnitude = jointLocalXROriginPosition.magnitude;

        forwardMagnitude = (lastJointPositionMagnitude - jointPositionMagnitude) * 1.0f;

        _lastJointLocalXROriginPosition = jointLocalXROriginPosition;

        // GameManager.changeDebugText.ChangeText($"{magnitudeDelta}");
    }
}