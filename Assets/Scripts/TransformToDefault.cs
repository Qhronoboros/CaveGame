using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class TransformToDefault : MonoBehaviour
{
    [SerializeField] private float _force = 10;
    private Rigidbody _rigidbody;

    public UnityEvent<Vector3> SettingVelocityStart;
    public UnityEvent<Vector3> SettingVelocityEnd;

    public UnityEvent<Vector3> SetPositionOrigin;
    public UnityEvent<Vector3> SetPositionMesh;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        transform.localRotation = quaternion.identity;
        _rigidbody.angularVelocity = Vector3.zero;

        float multiplier = _force * Mathf.Min(transform.localPosition.magnitude, 1.0f);

        _rigidbody.linearVelocity = (transform.rotation * -transform.localPosition).normalized * multiplier;

        // if (name == "Right Hand Tracking")
        // {
        //     Debug.Log(transform.localPosition);
        // }
        // GameManager.changeDebugText.ChangeText($"{_rigidbody.linearVelocity}");

        // Temporary Debugging
        // SettingVelocityStart?.Invoke(transform.position);
        // SettingVelocityEnd?.Invoke(transform.position + (transform.rotation * -transform.localPosition).normalized * multiplier);

        // SetPositionOrigin?.Invoke(-transform.localPosition);
        // SetPositionMesh?.Invoke(Vector3.zero);
    }
}