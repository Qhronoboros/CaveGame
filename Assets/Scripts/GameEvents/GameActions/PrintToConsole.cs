using UnityEngine;

public class PrintToConsole : MonoBehaviour
{
    [SerializeField] private string _printText;

    public void Execute() => Debug.Log(_printText);
}
