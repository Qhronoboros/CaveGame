using UnityEngine;
using UnityEngine.Events;

public class SimpleCollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    public UnityEvent OnColliding;
    public UnityEvent OnNotColliding;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        OnColliding.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        OnNotColliding.Invoke();
    }
}