using System.Collections;
using UnityEngine;

public class ModifyRotationPitch : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private int steps;

    public Vector3 desiredRotation;

    private Coroutine _coroutine;
    private bool _coroutineActive = false;

    IEnumerator LerpToRotation()
    {
        _coroutineActive = true;
        Quaternion startRotation = transform.rotation;
        for (int i = 1; i <= steps; i++)
        {
            Quaternion newRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(desiredRotation), (float)i / steps);
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

    public void SetDesiredRotation(Vector3 rotation)
    {
        if (rotation == desiredRotation) return;
        desiredRotation = rotation;

        ExitCoroutine();
        _coroutine = StartCoroutine(LerpToRotation());
    }
}
