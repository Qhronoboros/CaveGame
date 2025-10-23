using UnityEngine;
using UnityEngine.Events;

// Mainly used for rigidbodies with one collider
public class SimpleCollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    public UnityEvent<Collision> OnColliding;
    public UnityEvent<Collision> OnNotColliding;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        OnColliding?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;
        
        OnNotColliding?.Invoke(collision);
    }
}