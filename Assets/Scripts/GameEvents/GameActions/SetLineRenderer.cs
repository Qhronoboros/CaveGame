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
        _lineRenderer.SetPosition(0, localStartPosition);
    }
    public void SetLocalEndPosition(Vector3 localEndPosition)
    {
        _lineRenderer.SetPosition(1, localEndPosition);
    }

}