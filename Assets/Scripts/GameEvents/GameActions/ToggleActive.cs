using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void Toggle()
    {
        if (!gameObject.activeSelf) return;
        _gameObject.SetActive(!_gameObject.activeSelf);
    } 
}
