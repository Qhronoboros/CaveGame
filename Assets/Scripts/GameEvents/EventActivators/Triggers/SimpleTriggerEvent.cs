using UnityEngine;
using UnityEngine.Events;

// Mainly used for rigidbodies with one collider
public class SimpleTriggerEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    public UnityEvent<Collider> OnEntering;
    public UnityEvent<Collider> OnExiting;

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        OnEntering?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collisionObject = other.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        OnExiting?.Invoke(other);
    }
}