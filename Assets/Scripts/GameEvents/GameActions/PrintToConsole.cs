using UnityEngine;

public class PrintToConsole : MonoBehaviour
{
    [SerializeField] private string _printText;

    public void Execute()
    {
        if (!gameObject.activeSelf) return;
        Debug.Log(_printText);
    } 
}
