using UnityEngine;
using UnityEngine.Events;

public class KeyEvent : MonoBehaviour
{
    [SerializeField] private KeyCode Key;

    public UnityEvent KeyPressed;
    public UnityEvent KeyReleased;

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        
        if (Input.GetKeyDown(Key)) KeyPressed?.Invoke();
        else if (Input.GetKeyUp(Key)) KeyReleased?.Invoke();
    }
}