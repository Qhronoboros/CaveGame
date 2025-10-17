using UnityEngine;
using UnityEngine.Events;

public class SimpleCollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    // If false, only provide the first collision contact
    [SerializeField] private bool _callbackEveryCollision;

    private bool _isColliding = false;

    public UnityEvent<Collision> OnColliding;
    public UnityEvent<Collision> OnNotColliding;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        if (!_callbackEveryCollision && _isColliding) return;

        _isColliding = true;
        OnColliding.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        if (!_callbackEveryCollision && !_isColliding) return;

        _isColliding = false;
        OnNotColliding.Invoke(collision);
    }

    private void ResetValues()
    {
        _isColliding = false;
    }

    private void OnDisable() => ResetValues();
}