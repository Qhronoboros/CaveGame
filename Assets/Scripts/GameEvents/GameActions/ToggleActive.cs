using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void Toggle()
    {
        if (!gameObject.activeSelf) return;
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    public void Enable() => _gameObject.SetActive(true);
    public void Disable() => _gameObject.SetActive(false);
}
