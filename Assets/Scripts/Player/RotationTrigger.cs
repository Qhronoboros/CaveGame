using UnityEngine;
using UnityEngine.Events;

public class RotationTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _rotationObject;
    private ModifyRotationPitch _modifyRotationPitch;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Vector3 _inRotation;
    [SerializeField] private Vector3 _outRotation;

    private bool _passedFromFront;

    private void Awake()
    {
        _modifyRotationPitch = _rotationObject.GetComponent<ModifyRotationPitch>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf) return;

        GameObject collisionObject = other.gameObject;
        if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

        _passedFromFront = Vector3.Distance(other.gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 0.01f)
            <= Vector3.Distance(other.gameObject.transform.position, gameObject.transform.position - gameObject.transform.forward * 0.01f);

        Debug.Log($"Passed from front: {_passedFromFront}");

        if (_passedFromFront)
            _modifyRotationPitch.SetDesiredRotation(_outRotation);
        else
            _modifyRotationPitch.SetDesiredRotation(_inRotation);
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (!gameObject.activeSelf) return;
        
    //     GameObject collisionObject = other.gameObject;
    //     if (!LayerHelper.IsInLayerMask(_layerMask, collisionObject.layer)) return;

    // }
}