using UnityEngine;
using UnityEngine.XR.Hands;

public class CrawlingHand
{
    public bool isActive;
    public Handedness handedness;

    private XRHandJoint _joint;
    public XRHandJoint Joint
    {
        get { return _joint; }
        set
        {
            _joint = value;
            // UpdateTotalVelocity();
        }
    }
    private Vector2 _lastJointLocalXROriginPosition;

    public CrawlingHand(Handedness handedness)
    {
        isActive = false;
        this.handedness = handedness;
    }

    public void SetJoint(XRHandSubsystem subsystem)
    {
        switch (handedness)
        {
            case Handedness.Left:
                Joint = subsystem.leftHand.GetJoint(XRHandJointID.Palm);
                break;
            case Handedness.Right:
                Joint = subsystem.leftHand.GetJoint(XRHandJointID.Palm);
                break;
            default:
                Debug.LogError("Handedness is invalid");
                return;
        }
    }
}