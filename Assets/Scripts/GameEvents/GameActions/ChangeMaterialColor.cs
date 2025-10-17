using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;
    [SerializeField] private Renderer _renderer;
    private Material _material;

    private void Awake()
    {
        _material = _renderer.material;
        _material.color = _firstColor;
    }

    public void SetFirstColor() => _material.color = _firstColor;
    public void SetSecondColor() => _material.color = _secondColor;
}
