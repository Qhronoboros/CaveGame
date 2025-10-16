using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private List<GameObject> _collidingObjects = new List<GameObject>();

    public UnityEvent OnColliding;
    public UnityEvent OnNotColliding;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        _collidingObjects.Add(collisionObject);

        // Debug.Log($"Add {collision.gameObject.name} {_collidingObjects.Count}");

        if (_collidingObjects.Count != 1) return;

        OnColliding.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)
                || !_collidingObjects.Contains(collisionObject)) return;

        _collidingObjects.Remove(collisionObject);

        // Debug.Log($"Remove {collision.gameObject.name} {_collidingObjects.Count}");

        if (_collidingObjects.Count != 0) return;

        OnNotColliding.Invoke();
    }
}