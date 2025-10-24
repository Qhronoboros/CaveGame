using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public void SetTransformPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetLocalTransformPosition(Vector3 localPosition)
    {
        transform.localPosition = localPosition;
    }
}