using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class AlignToSurface : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private int steps;
    
    private Quaternion desiredRotation;

    private Coroutine _coroutine;
    private bool _coroutineActive = false;

    IEnumerator LerpToRotation()
    {
        _coroutineActive = true;
        Quaternion startRotation = transform.rotation;
        for (int i = 1; i <= steps; i++)
        {
            Quaternion newRotation = Quaternion.Lerp(startRotation, desiredRotation, (float)i / steps);
            transform.eulerAngles = new Vector3(newRotation.eulerAngles.x, 0.0f, newRotation.eulerAngles.z);
            yield return new WaitForSeconds(0.01f);
        }

        _coroutineActive = false;
    }

    private void ExitCoroutine()
    {
        if (_coroutineActive)
        {
            _coroutineActive = false;
            StopCoroutine(_coroutine);
        }
    }

    private void Update()
    {
        // ! Just have a trigger collider for changing the rotation instead of this 
        return;
    
        Vector3 bottomPosition;
        if (_characterController == null)
        {
            bottomPosition = _collider.gameObject.transform.position + _collider.center +
                Vector3.up * (_collider.height * gameObject.transform.lossyScale.y);
        }
        else
        {
            bottomPosition = _characterController.gameObject.transform.position + _characterController.center +
                Vector3.up * (_characterController.height - _characterController.radius);
            
        }
    
        if (Physics.Raycast(new Ray(bottomPosition, -transform.up)
            , out RaycastHit hit, 0.2f, _layerMask))
        {   
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Debug.Log($"{hit.collider.name} - {hit.normal}");
            
            if (rotation == desiredRotation) return;

            desiredRotation = rotation;

            // Debug.Log(desiredRotation.eulerAngles);
            
            ExitCoroutine();
            _coroutine = StartCoroutine(LerpToRotation());
        }
    }

    private void OnDisable() => ExitCoroutine();
}
