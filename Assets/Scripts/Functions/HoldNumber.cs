using UnityEngine;
using UnityEngine.Events;

public class HoldNumber : MonoBehaviour
{
    [SerializeField] private HoldNumberFunction _holdNumberFunction;
    [SerializeField] private float _heldValue;

    public UnityEvent<float> NewHeldValue;

    private void Awake()
    {
        if (_holdNumberFunction == HoldNumberFunction.MIN)
            _heldValue = Mathf.Infinity;
    }

    public void CompareValues(float value)
    {
        if (!gameObject.activeSelf) return;

        bool result = false;

        switch (_holdNumberFunction)
        {
            case HoldNumberFunction.MIN:
                result = value < _heldValue;
                break;
            case HoldNumberFunction.MAX:
                result = value > _heldValue;
                break;
        }

        if (!result) return;

        _heldValue = value;
        NewHeldValue?.Invoke(value);
    }
}
