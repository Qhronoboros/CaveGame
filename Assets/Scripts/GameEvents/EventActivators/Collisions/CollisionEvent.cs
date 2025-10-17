using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Class works with compoundColliders
public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    // Dictionary contains childCollider and contactList pair
    private Dictionary<Collider, List<GameObject>> _childColliderDict = new Dictionary<Collider, List<GameObject>>();

    // PreviousContactList is for later expansion if needed
    private List<GameObject> _previousContactList = new List<GameObject>();
    private List<GameObject> _contactList = new List<GameObject>();

    private bool _isColliding = false;

    public UnityEvent OnColliding;
    public UnityEvent OnNotColliding;

    private void FixedUpdate() => EvaluateContactList();
    
    private void EvaluateContactList()
    {
        if (_isColliding && _contactList.Count == 0)
        {
            _isColliding = false;
            OnNotColliding.Invoke();
        }

        _previousContactList = _contactList;
        _contactList.Clear();
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        if (_isColliding) return;

        _isColliding = true;
        OnColliding.Invoke();
    }

    private void ResetValues()
    {
        if (_isColliding)
        {
            _isColliding = false;
            OnNotColliding.Invoke();
        }

        _previousContactList.Clear();
        _contactList.Clear();
    }

    private void OnDisable() => ResetValues();
}