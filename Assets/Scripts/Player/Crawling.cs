using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class Crawling : MonoBehaviour
{
    public XROrigin origin;
    private XRHandSubsystem _subsystem;
    [SerializeField] private ContinuousMoveProvider _continuousMoveProvider;
    [SerializeField] private ContinuousTurnProvider _continuousTurnProvider;

    private CrawlingHand _leftCrawlingHand;
    private CrawlingHand _rightCrawlingHand;

    private float _movementMultiplier;

    private void Awake()
    {
        _leftCrawlingHand = new CrawlingHand(this, Handedness.Left);
        _rightCrawlingHand = new CrawlingHand(this, Handedness.Right);
        _movementMultiplier = 1.0f;
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
        SetHandInactive(handTrackingEvents.handedness);
        UpdateTotalVelocity();
    }

    public void JointsUpdated(XRHandJointsUpdatedEventArgs eventArgs)
    {
        XRHandJoint handJoint = eventArgs.hand.GetJoint(XRHandJointID.Palm);
        CrawlingHand crawlingHand = GetCrawlingHand(handJoint.handedness);
        crawlingHand.SetJoint(handJoint);

        UpdateTotalVelocity();
    }

    private void SetHandInactive(Handedness handedness)
    {
        CrawlingHand crawlingHand = GetCrawlingHand(handedness);
        crawlingHand.SetInactive();
    }

    private CrawlingHand GetCrawlingHand(Handedness handedness)
    {
        switch (handedness)
        {
            case Handedness.Left:
                return _leftCrawlingHand;
            case Handedness.Right:
                return _rightCrawlingHand;
            default:
                Debug.LogError("Handedness is invalid");
                return null;
        }
    }

    public void UpdateTotalVelocity()
    {
        // Vector2 movement = new Vector2(0.0f, (_leftCrawlingHand.forwardMagnitude + _rightCrawlingHand.forwardMagnitude) * _movementMultiplier);
        // Vector2 turn = new Vector2((_leftCrawlingHand.turnAmount + _rightCrawlingHand.turnAmount) * _movementMultiplier, 0.0f);

        // _continuousMoveProvider.leftHandMoveInput.manualValue = movement;
        // _continuousTurnProvider.leftHandTurnInput.manualValue = turn;
        
        _continuousMoveProvider.leftHandMoveInput.manualValue = new Vector2(0.0f, _leftCrawlingHand.forwardMagnitude * _movementMultiplier);
        _continuousTurnProvider.leftHandTurnInput.manualValue = new Vector2(_leftCrawlingHand.turnAmount * _movementMultiplier, 0.0f);

        _continuousMoveProvider.rightHandMoveInput.manualValue = new Vector2(0.0f, _rightCrawlingHand.forwardMagnitude * _movementMultiplier);
        _continuousTurnProvider.rightHandTurnInput.manualValue = new Vector2(_rightCrawlingHand.turnAmount * _movementMultiplier, 0.0f);

        // Debug.Log($"{_leftCrawlingHand.turnAmount} - {_rightCrawlingHand.turnAmount}");

        // _continuousMoveProvider.moveSpeed = 1.0f;
        // _continuousTurnProvider.turnSpeed = 1.0f;
    }

    public void SetMovementMultiplier(float value) => _movementMultiplier = 1.0f - value;

    private Vector2 GetManualInputValue(Vector3 handVelocity, float handMagnitude, float maxMagnitude)
    {
        if (handMagnitude == 0.0f)
            return Vector2.zero;

        float fractionMagnitude = handMagnitude / maxMagnitude;
        Vector3 handVelocityNormalized = handVelocity.normalized;
        return new Vector2(handVelocityNormalized.x, handVelocityNormalized.z) * fractionMagnitude;
    }
}