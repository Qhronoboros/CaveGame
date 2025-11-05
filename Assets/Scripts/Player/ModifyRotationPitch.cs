using System.Collections;
using UnityEngine;

public class ModifyRotationPitch : MonoBehaviour
{
    [SerializeField] private int steps;
        private Vector3 _desiredRotation;

    private Coroutine _coroutine;
    private bool _coroutineActive = false;

    IEnumerator LerpToRotation()
    {
        Debug.Log("Starting");
        _coroutineActive = true;
        Quaternion startRotation = transform.rotation;
        for (int i = 1; i <= steps; i++)
        {
            Quaternion newRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(_desiredRotation), (float)i / steps);
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
        if (rotation == _desiredRotation) return;
        _desiredRotation = rotation;

        ExitCoroutine();
        _coroutine = StartCoroutine(LerpToRotation());
    }
}
