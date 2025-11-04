using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Class works with compoundColliders
// Sends callbacks whenever something first enters it
// And when all colliders exit
public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    // Dictionary contains childCollider and contactList pair
    private Dictionary<Collider, List<GameObject>> _childColliderDict = new Dictionary<Collider, List<GameObject>>();

    // PreviousContactList is for later expansion if needed
    private List<GameObject> _previousContactList = new List<GameObject>();
    private List<GameObject> _contactList = new List<GameObject>();

    private bool _isTriggering = false;

    private Coroutine _coroutine;
    private bool _coroutineActive = false;

    // Gets invoked only once when a collider enters the trigger area
    // Needs to wait for all external colliders to exit, before invoking again
    public UnityEvent OnTriggering;
    // Gets invoked when all external colliders exit the trigger area
    public UnityEvent OnNotTriggering;

    private void Start() => _coroutine = StartCoroutine(EvaluateContactList());

    // After OnCollisionStay, check if there are any collisions
    IEnumerator EvaluateContactList()
    {
        _coroutineActive = true;
        while (_coroutineActive)
        {
            yield return new WaitForFixedUpdate();

            if (_isTriggering && _contactList.Count == 0)
            {
                _isTriggering = false;
                OnNotTriggering?.Invoke();
            }

            _previousContactList = _contactList;
            _contactList.Clear();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!gameObject.activeSelf) return;

        GameObject collisionObject = other.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        if (!_contactList.Contains(collisionObject))
            _contactList.Add(collisionObject);

        if (_isTriggering) return;

        _isTriggering = true;
        OnTriggering?.Invoke();
    }

    private void ResetValues()
    {
        _coroutineActive = false;
        StopCoroutine(_coroutine);

        if (_isTriggering)
        {
            _isTriggering = false;
            OnNotTriggering?.Invoke();
        }

        _previousContactList.Clear();
        _contactList.Clear();
    }

    private void OnEnable() => Start();
    private void OnDisable() => ResetValues();
}