using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LogicGateEvaluator : MonoBehaviour
{
    [SerializeField] private LogicGate _logicGate;
    [SerializeField] private GameObject _returnedObject;

    [SerializeField] private int _operandAmount;
    private List<bool> _operandList = new List<bool>();

    private bool _lastResult = false;

    public UnityEvent<GameObject> EvaluateTrue;
    public UnityEvent<GameObject> EvaluateFalse;

    private void Awake()
    {
        if (_operandAmount < 2)
            Debug.LogError($"OperandAmount {_operandAmount} is invalid");

        for (int i = 0; i < _operandAmount; i++)
            _operandList.Add(false);
    }

    // Hack for getting two arguments in the Unity inspector
    // Needs to have a decimal
    // Left side of the separator is the index
    // Right side of the separator is the bool value (.1 or .0)
    public void SetOperand(float indexAndValue)
    {
        // Not elegant, but it works
        string[] indexAndValueString = indexAndValue.ToString("0.00").Split(".");
        int index = int.Parse(indexAndValueString[0]);
        bool value = Convert.ToBoolean(int.Parse(indexAndValueString[1][0].ToString()));

        if (index < 0 || index >= _operandAmount)
        {
            Debug.LogError($"Given index {index} in SetOperand() is invalid");
            return;
        }

        _operandList[index] = value;
        Evaluate();
    }

    private void Evaluate()
    {
        if (_operandList.Aggregate(GetLambdaExpression(_logicGate)) != _lastResult)
        {
            _lastResult = !_lastResult;

            if (_lastResult) EvaluateTrue.Invoke(_returnedObject);
            else EvaluateFalse.Invoke(_returnedObject);
        }
    }

    private Func<bool, bool, bool> GetLambdaExpression(LogicGate logicGate)
    {
        switch (logicGate)
        {
            case LogicGate.OR:
                return (x, y) => x || y;
            case LogicGate.NOR:
                return (x, y) => !(x || y);
            case LogicGate.XOR:
                return (x, y) => !x == y;
            case LogicGate.XNOR:
                return (x, y) => x == y;
            case LogicGate.AND:
                return (x, y) => x && y;
            case LogicGate.NAND:
                return (x, y) => !(x && y);
            default:    // Default OR Gate
                return (x, y) => x || y;
        }
    }
}