using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class MathProvider : MonoBehaviour
{
    [SerializeField] private MathFunction _mathFunction;
    private List<float> _operandList = new List<float>();

    private float _lastResult;

    public UnityEvent<float> AppliedMath;


    public void ReceiveData(float value)
    {
        if (!gameObject.activeSelf) return;

        _operandList.Add(value);
        Evaluate();
    }

    private void Evaluate()
    {
        // ! Will only work if both values come from an update function
        if (_operandList.Count < 2)
        {
            // Debug.Log("Smaller than 2");
            return;
        }

        float result = _operandList.Aggregate(GetLambdaExpression(_mathFunction));

        _operandList.Clear();

        if (result == _lastResult) return;
        _lastResult = result;

        Debug.Log($"{result}");

        AppliedMath?.Invoke(result);
    }

    private Func<float, float, float> GetLambdaExpression(MathFunction mathFunction)
    {
        switch (mathFunction)
        {
            case MathFunction.MAX:
                return (x, y) => Mathf.Max(x, y);
            case MathFunction.MIN:
                return (x, y) => Mathf.Min(x, y);
            case MathFunction.AVERAGE:
                return (x, y) => (x + y) / 2.0f;
            default:    // Default MAX Function
                return (x, y) => Mathf.Max(x, y);
        }
    }

}