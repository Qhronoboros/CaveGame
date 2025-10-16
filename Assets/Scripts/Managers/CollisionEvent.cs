using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _collisionColor;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Renderer _renderer;
    private Material _material;
    private List<GameObject> _collidingObjects = new List<GameObject>();

    public UnityEvent<GameObject> OnColliding;
    public UnityEvent<GameObject> OnNotColliding;

    private void Start()
    {
        _material = _renderer.material;
        _material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        _collidingObjects.Add(collisionObject);

        if (_collidingObjects.Count != 1) return;

        _material.color = _collisionColor;
        OnColliding.Invoke(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)
                || !_collidingObjects.Contains(collisionObject)) return;

        _collidingObjects.Remove(collisionObject);

        if (_collidingObjects.Count != 0) return;

        _material.color = _defaultColor;
        OnNotColliding.Invoke(gameObject);
    }
}