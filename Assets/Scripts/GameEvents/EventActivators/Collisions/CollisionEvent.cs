using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Class works with compoundColliders
// Sends callbacks whenever something first collides with it
// And when all colliders exit
public class CollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    // Dictionary contains childCollider and contactList pair
    private Dictionary<Collider, List<GameObject>> _childColliderDict = new Dictionary<Collider, List<GameObject>>();

    // PreviousContactList is for later expansion if needed
    private List<GameObject> _previousContactList = new List<GameObject>();
    private List<GameObject> _contactList = new List<GameObject>();

    private bool _isColliding = false;

    private Coroutine _coroutine;
    private bool _coroutineActive = false;

    // Gets invoked only once when a collider collides with the object
    // Needs to wait for all external colliders stop colliding before invoking again
    public UnityEvent OnColliding;
    // Gets invoked when all external colliders stop colliding with this object
    public UnityEvent OnNotColliding;

    private void Start() => _coroutine = StartCoroutine(EvaluateContactList());

    // After OnCollisionStay, check if there are any collisions
    IEnumerator EvaluateContactList()
    {
        _coroutineActive = true;
        while (_coroutineActive)
        {
            yield return new WaitForFixedUpdate();

            if (_isColliding && _contactList.Count == 0)
            {
                _isColliding = false;
                OnNotColliding?.Invoke();
            }

            _previousContactList = _contactList;
            _contactList.Clear();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!gameObject.activeSelf) return;
        
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        if (!_contactList.Contains(collisionObject))
            _contactList.Add(collisionObject);

        if (_isColliding) return;

        _isColliding = true;
        OnColliding?.Invoke();
    }

    private void ResetValues()
    {
        _coroutineActive = false;
        StopCoroutine(_coroutine);

        if (_isColliding)
        {
            _isColliding = false;
            OnNotColliding?.Invoke();
        }

        _previousContactList.Clear();
        _contactList.Clear();
    }

    private void OnEnable() => Start();
    private void OnDisable() => ResetValues();
}