using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ImitateTransform : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(Vector3.zero);
        _rb.MoveRotation(quaternion.identity);
    }
}
