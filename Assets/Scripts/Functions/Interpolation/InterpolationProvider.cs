using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class InterpolationProvider : MonoBehaviour
{
    [SerializeField] private InterpolationFunction _interpolation;
    [SerializeField] private float _minimum;
    [SerializeField] private float _maximum;

    public UnityEvent<float> ValueCalculated;

    public void SetMinimum(float value) => _minimum = value;
    public void SetMaxmimum(float value) => _maximum = value;

    public void Calculate(float value)
    {
        float calculatedValue = float.PositiveInfinity;

        switch (_interpolation)
        {
            case InterpolationFunction.LERP:
                calculatedValue = Mathf.Lerp(_minimum, _maximum, value);
                break;
            case InterpolationFunction.LERP_UNCLAMPED:
                calculatedValue = Mathf.LerpUnclamped(_minimum, _maximum, value);
                break;
            case InterpolationFunction.SMOOTH_STEP:
                calculatedValue = Mathf.SmoothStep(_minimum, _maximum, value);
                break;
            case InterpolationFunction.INVERSE_LERP:
                calculatedValue = Mathf.InverseLerp(_minimum, _maximum, value);
                break;
            case InterpolationFunction.LERP_ANGLE:
                calculatedValue = Mathf.LerpAngle(_minimum, _maximum, value);
                break;
        }

        if (calculatedValue == float.PositiveInfinity) return;
        Debug.Log(calculatedValue);
        GameManager.changeDebugText.ChangeText($"{calculatedValue:0.000}");
        ValueCalculated?.Invoke(calculatedValue);
    }
}