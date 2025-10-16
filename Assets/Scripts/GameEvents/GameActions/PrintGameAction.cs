using UnityEngine;

public class PrintGameAction : MonoBehaviour, IGameAction
{
    [SerializeField] private string _printText;

    public void Execute() => Debug.Log(_printText);
}
