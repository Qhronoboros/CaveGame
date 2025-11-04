using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public void SetTransformPosition(Vector3 position)
    {
        if (!gameObject.activeSelf) return;
        transform.position = position;
    }

    public void SetLocalTransformPosition(Vector3 localPosition)
    {
        if (!gameObject.activeSelf) return;
        transform.localPosition = localPosition;
    }
}