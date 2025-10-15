using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _collisionColor;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Renderer _renderer;
    private Material _material;

    public UnityEvent<GameObject> Colliding;
    public UnityEvent<GameObject> NotColliding;

    // The amount of colliders the object is colliding with
    private int _collisionCount = 0;

    private void Awake()
    {
        _material = _renderer.material;
        _material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!LayerHelper.IsInLayerMask(_layerMask, collision.gameObject.layer)) return;

        if (++_collisionCount > 1) return;

        _material.color = _collisionColor;
        Colliding.Invoke(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!LayerHelper.IsInLayerMask(_layerMask, collision.gameObject.layer)) return;

        if (--_collisionCount < 1) return;

        _material.color = _defaultColor;
        NotColliding.Invoke(gameObject);
    }
}