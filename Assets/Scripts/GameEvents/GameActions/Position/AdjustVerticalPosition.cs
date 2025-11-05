using UnityEngine;

public class AdjustVerticalPosition : MonoBehaviour
{
    private Transform _transform;

    private void Awake() => _transform = GetComponent<Transform>();

    public void ChangeVertical(float adjustment)
    {
        _transform.localPosition = _transform.localPosition + Vector3.up * adjustment;
    }
}