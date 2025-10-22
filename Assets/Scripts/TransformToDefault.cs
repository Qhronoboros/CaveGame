using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TransformToDefault : MonoBehaviour
{
    [SerializeField] private float _force = 10;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.localRotation = quaternion.identity;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 localOriginPositionWS = transform.position - transform.localPosition;

        // Bug has to do something over here
        float multiplier = _force * Mathf.Min(Vector3.Distance(localOriginPositionWS, transform.position), 1.0f);

        if (name == "Right Hand Tracking")
        {
            Debug.Log(multiplier);
        }

        if (Vector3.Distance(localOriginPositionWS, transform.position) > 2.0f)
        {
            // Debug.Log("Whoops");
            _rigidbody.linearVelocity = Vector3.zero;
        }
        else
        {
            _rigidbody.linearVelocity = (localOriginPositionWS - transform.position).normalized * multiplier;
            // GameManager.changeDebugText.ChangeText($"{_rigidbody.linearVelocity}");
        }
    }
}