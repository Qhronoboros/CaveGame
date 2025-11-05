using UnityEngine;
using UnityEngine.Events;

public class ActiveEvent : MonoBehaviour
{
    public UnityEvent Active;
    public UnityEvent Inactive;

    private void OnEnable() => Active?.Invoke();
    private void OnDisable() => Inactive?.Invoke();
}
