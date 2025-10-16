using UnityEngine;
using UnityEngine.Events;

public class ORGate : MonoBehaviour
{
    [SerializeField] private GameObject _returnedObject;
    [SerializeField] private int _maxCount;
    private int _count;

    public UnityEvent<GameObject> True;
    public UnityEvent<GameObject> False;

    public void AddCount()
    {
        if (_count++ == 0)
        {
            True.Invoke(_returnedObject);
            // Debug.Log("ORGate True");
        }
        else if (++_count > _maxCount)
        {
            Debug.LogError("Count is higher than maxCount");
            _count = _maxCount;
            return;
        }
    }

    public void SubtractCount()
    {
        if (--_count == 0)
        {
            False.Invoke(_returnedObject);
            // Debug.Log("ORGate False");
        }
        else if (_count < 0)
        {
            Debug.LogError("Count is lower than 0");
            _count = 0;
            return;
        }
    }
}