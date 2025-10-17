using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/*  Order of operations:
- Empty contactLists of childColliders (FixedUpdate)
- Receive all contacts for each childColliders and put them in contactLists (OnCollisionStay)
- Check if childColliders are 
*/

// Class works with compoundColliders
// ! Not tested
public class ComplexCollisionEvent : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    // Dictionary contains childCollider and contactList pair
    private Dictionary<Collider, List<GameObject>> _childColliderDict = new Dictionary<Collider, List<GameObject>>();

    private bool _isColliding = false;

    private Coroutine _ValidateCoroutine;

    public UnityEvent OnColliding;
    public UnityEvent OnNotColliding;

    private void Awake()
    {
        List<Collider> childColliders = GetComponentsInChildren<Collider>(true).ToList();

        foreach (Collider collider in childColliders)
            _childColliderDict.Add(collider, new List<GameObject>());
    }

    private bool GetContactList(Collider childCollider, out List<GameObject> contactList)
    {
        if (_childColliderDict.TryGetValue(childCollider, out contactList)) { return true; }
        else { Debug.LogError($"Could not find contactList with childCollider: {childCollider}"); return false; }
    }

    private void FixedUpdate()
    {
        EmptyContactLists();
    }

    private void EmptyContactLists()
    {
        foreach (List<GameObject> contactList in _childColliderDict.Values)
            contactList.Clear();
    }

    // Checks if still colliding after physics update
    private IEnumerator ValidateAfterPhysics()
    {
        while (_isColliding)
        {
            yield return new WaitForFixedUpdate();
            if (ValidateCollisionActivity())
            {
                _isColliding = false;
                OnNotColliding.Invoke();
            }
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     GameObject collisionObject = collision.gameObject;
    //     if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

    //     GetContactList(collision.GetContact(0).thisCollider, out List<GameObject> contactList);
    //     if (contactList.Contains(collisionObject)) return;

    //     contactList.Add(collisionObject);

    //     if (_isColliding) return;

    //     _isColliding = true;
    //     OnColliding.Invoke();
    // }

    // * Not elegant, but OnCollisionExit does not provide collision contacts
    // * So knowing which childCollider called it is impossible
    private void OnCollisionStay(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        GetContactList(collision.GetContact(0).thisCollider, out List<GameObject> contactList);
        if (contactList.Contains(collisionObject)) return;

        contactList.Add(collisionObject);

        if (_isColliding) return;

        _isColliding = true;
        OnColliding.Invoke();

        _ValidateCoroutine = StartCoroutine(ValidateAfterPhysics());
    }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (!_isColliding) return;

    //     GameObject collisionObject = collision.gameObject;
    //     if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

    //     GetContactList(collision.GetContact(0).thisCollider, out List<GameObject> contactList);
    //     if (!contactList.Contains(collisionObject)) return;

    //     contactList.Remove(collisionObject);

    //     if (ValidateCollisionActivity()) return;

    //     _isColliding = false;
    //     OnNotColliding.Invoke();
    // }

    // Checks whether a childCollider is still colliding with something
    private bool ValidateCollisionActivity()
    {
        foreach (List<GameObject> contactList in _childColliderDict.Values)
            if (contactList.Count > 0) return true;
        return false;
    }

    private void ResetValues()
    {
        if (_isColliding)
        {
            _isColliding = false;
            OnNotColliding.Invoke();
            StopCoroutine(_ValidateCoroutine);
        }

        foreach (List<GameObject> contactList in _childColliderDict.Values)
            contactList.Clear();
    }

    private void OnDisable() => ResetValues();
}