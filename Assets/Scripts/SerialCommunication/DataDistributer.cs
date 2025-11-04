using UnityEngine;
using UnityEngine.Events;

public class DataDistributer : MonoBehaviour
{
    public int data;
    public bool hasNewData;

    public UnityEvent<float> EmittingData;

    private void Update()
    {
        if (!hasNewData) return;

        hasNewData = false;
        // Debug.Log($"Receiving Data: {data}");
        // GameManager.changeDebugText.ChangeText($"{data}");
        EmittingData?.Invoke(data);
    }
}