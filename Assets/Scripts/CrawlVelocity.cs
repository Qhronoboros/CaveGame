using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class CrawlVelocity : MonoBehaviour
{
    private XRHandSubsystem _subsystem;
    [SerializeField] private XROrigin _origin;
    [SerializeField] private ContinuousMoveProvider _continuousMoveProvider;
    [SerializeField] private ContinuousTurnProvider _continuousTurnProvider;

    private XRHandJoint _leftHandJoint;
    public XRHandJoint LeftHandJoint
    {
        get { return _leftHandJoint; }
        set
        {
            _leftHandJoint = value;
            UpdateTotalVelocity();
        }
    }
    private Vector2 _leftLastJointLocalXROriginPosition;

    private XRHandJoint _rightHandJoint;
    public XRHandJoint RightHandJoint
    {
        get { return _rightHandJoint; }
        set
        {
            _rightHandJoint = value;
            UpdateTotalVelocity();
        }
    }
    private Vector2 _rightLastJointLocalXROriginPosition;

    private void Awake()
    {
        LeftHandJoint = default;
        RightHandJoint = default;
    }

    private void Start()
    {
        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        // There should only be one subsystem
        _subsystem = handSubsystems[0];
    }

    public void StartTrackingJoint(GameObject trackingObject)
    {
        if (!trackingObject.TryGetComponent(out XRHandTrackingEvents handTrackingEvents))
        {
            Debug.LogError($"{trackingObject.name} gameObject does not have XRHandTrackingEvents component");
            return;
        }

        handTrackingEvents.jointsUpdated.AddListener(JointsUpdated);
    }

    public void StopTrackingJoint(GameObject trackingObject)
    {
        if (!trackingObject.TryGetComponent(out XRHandTrackingEvents handTrackingEvents))
        {
            Debug.LogError($"{trackingObject.name} gameObject does not have XRHandTrackingEvents component");
            return;
        }

        handTrackingEvents.jointsUpdated.RemoveListener(JointsUpdated);
        SetHandJoint(handTrackingEvents.handedness, true);
    }

    public void JointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        XRHand hand = eventArgs.hand;
        XRHandJoint handJoint = hand.GetJoint(XRHandJointID.Palm);
        
        handJoint.TryGetLinearVelocity(out Vector3 linearVelocity);
        
        // Debug.Log($"{hand.handedness} linearVelocity: {linearVelocity}");

        SetHandJoint(hand.handedness, false);
    }

    private void SetHandJoint(Handedness handedness, bool reset)
    {
        switch (handedness)
        {
            case Handedness.Left:
                LeftHandJoint = reset ? default : _subsystem.leftHand.GetJoint(XRHandJointID.Palm);
                _leftLastJointLocalXROriginPosition = reset ? Vector2.zero : _leftLastJointLocalXROriginPosition;
                _continuousMoveProvider.leftHandMoveInput.manualValue = reset ? Vector2.zero : _continuousMoveProvider.leftHandMoveInput.manualValue;
                _continuousTurnProvider.leftHandTurnInput.manualValue = reset ? Vector2.zero : _continuousTurnProvider.leftHandTurnInput.manualValue;
                break;
            case Handedness.Right:
                RightHandJoint = reset ? default : _subsystem.rightHand.GetJoint(XRHandJointID.Palm);
                _rightLastJointLocalXROriginPosition = reset ? Vector2.zero : _rightLastJointLocalXROriginPosition;
                _continuousMoveProvider.rightHandMoveInput.manualValue = reset ? Vector2.zero : _continuousMoveProvider.rightHandMoveInput.manualValue;
                _continuousTurnProvider.rightHandTurnInput.manualValue = reset ? Vector2.zero : _continuousTurnProvider.rightHandTurnInput.manualValue;
                break;
            default:
                Debug.LogError("Handedness is invalid");
                return;
        }
    }

    public void UpdateTotalVelocity()
    {
        if (LeftHandJoint == default)
        {
            _continuousMoveProvider.leftHandMoveInput.manualValue = Vector2.zero;
            _continuousTurnProvider.leftHandTurnInput.manualValue = Vector2.zero;
        }
        else
        {
            LeftHandJoint.TryGetPose(out Pose leftJointPose);
            Vector2 leftJointLocalXROriginPosition = new Vector2(leftJointPose.position.x, leftJointPose.position.z);

            if (_leftLastJointLocalXROriginPosition == Vector2.zero)
            {
                _leftLastJointLocalXROriginPosition = leftJointLocalXROriginPosition;
                return;
            }

            float turnAmount = Vector2.SignedAngle(_leftLastJointLocalXROriginPosition, leftJointLocalXROriginPosition);
            _continuousTurnProvider.leftHandTurnInput.manualValue = new Vector2(turnAmount, 0.0f);

            float leftLastJointPositionMagnitude = _leftLastJointLocalXROriginPosition.magnitude;
            float leftJointPositionMagnitude = leftJointLocalXROriginPosition.magnitude;

            float magnitudeDelta = (leftLastJointPositionMagnitude - leftJointPositionMagnitude) * 1.0f;

            _leftLastJointLocalXROriginPosition = leftJointLocalXROriginPosition;

            // GameManager.changeDebugText.ChangeText($"{magnitudeDelta}");

            _continuousMoveProvider.leftHandMoveInput.manualValue = new Vector2(0.0f, magnitudeDelta);
        }

        if (RightHandJoint == default)
        {
            _continuousMoveProvider.rightHandMoveInput.manualValue = Vector2.zero;
            _continuousTurnProvider.rightHandTurnInput.manualValue = Vector2.zero;
        }
        else
        {
            RightHandJoint.TryGetPose(out Pose rightJointPose);
            Vector2 rightJointLocalXROriginPosition = new Vector2(rightJointPose.position.x, rightJointPose.position.z);

            if (_rightLastJointLocalXROriginPosition == Vector2.zero)
            {
                _rightLastJointLocalXROriginPosition = rightJointLocalXROriginPosition;
                return;
            }

            float turnAmount = Vector2.SignedAngle(_rightLastJointLocalXROriginPosition, rightJointLocalXROriginPosition);
            _continuousTurnProvider.rightHandTurnInput.manualValue = new Vector2(turnAmount, 0.0f);

            float rightLastJointPositionMagnitude = _rightLastJointLocalXROriginPosition.magnitude;
            float rightJointPositionMagnitude = rightJointLocalXROriginPosition.magnitude;

            float magnitudeDelta = (rightLastJointPositionMagnitude - rightJointPositionMagnitude) * 1.0f;

            _rightLastJointLocalXROriginPosition = rightJointLocalXROriginPosition;

            _continuousMoveProvider.rightHandMoveInput.manualValue = new Vector2(0.0f, magnitudeDelta);
        }

        // _continuousMoveProvider.moveSpeed = 1.0f;
    }

    private Vector2 GetManualInputValue(Vector3 handVelocity, float handMagnitude, float maxMagnitude)
    {
        if (handMagnitude == 0.0f)
            return Vector2.zero;

        float fractionMagnitude = handMagnitude / maxMagnitude;
        Vector3 handVelocityNormalized = handVelocity.normalized;
        return new Vector2(handVelocityNormalized.x, handVelocityNormalized.z) * fractionMagnitude;
    }
}