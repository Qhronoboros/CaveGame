using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SetLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, Vector3.zero);
    }

    public void SetLocalStartPosition(Vector3 localStartPosition)
    {
        if (!gameObject.activeSelf) return;
        _lineRenderer.SetPosition(0, localStartPosition);
    }
    public void SetLocalEndPosition(Vector3 localEndPosition)
    {
        if (!gameObject.activeSelf) return;
        _lineRenderer.SetPosition(1, localEndPosition);
    }

}