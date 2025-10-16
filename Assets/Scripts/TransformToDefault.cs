using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TransformToDefault : MonoBehaviour
{
    private Rigidbody _rb;
    private float _force = 10;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
        _rb.linearVelocity = -_rb.position.normalized * _force * Vector3.Distance(Vector3.zero, _rb.position);
    }
}