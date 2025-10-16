using System;
using UnityEngine;
using UnityEngine.Events;

public class LogicGateEvaluator : MonoBehaviour
{
    [SerializeField] private LogicGate _logicGate;
    [SerializeField] private GameObject _returnedObject;

    private bool _firstOperand = false;
    private bool _secondOperand = false;

    private bool _lastResult = false;

    public UnityEvent<GameObject> EvaluateTrue;
    public UnityEvent<GameObject> EvaluateFalse;

    public void SetFirstOperand(bool value)
    {
        _firstOperand = value;
        Evaluate();
    }
    
    public void SetSecondOperand(bool value)
    {
        _secondOperand = value;
        Evaluate();
    }

    private void Evaluate()
    {
        if (GetLambdaExpression(_logicGate)(_firstOperand, _secondOperand) != _lastResult)
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