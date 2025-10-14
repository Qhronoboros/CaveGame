using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;
using System.Threading;

public class SerialCommunication : MonoBehaviour
{
    private SerialPort _serialPort;
    private Thread _thread;
    private bool _isLooping;
    public UnityEvent<int> dataReceived;

    private void Awake()
    {
        _serialPort = new SerialPort("COM5", 9600);
        _serialPort.Open();

        ThreadLoop();
    }

    private bool IsLooping()
    {
        lock (this) { return _isLooping; }
    }

    private void ThreadLoop()
    {
        _thread = new Thread(ThreadLoop);
        _thread.Start();

        while (IsLooping())
        {
            if (!int.TryParse(_serialPort.ReadLine(), out int data))
            {
                Debug.LogError("Received Data is not an integer");
                return;
            }

            Debug.Log($"Receiving Data: {data}");
            dataReceived.Invoke(data);
        }
        
        if (_serialPort.IsOpen)
            _serialPort.Close();
    }

    private void StopThread()
    {
        lock (this) { _isLooping = false; }
    }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        Debug.Log("Receiving Data");

        SerialPort sp = (SerialPort)sender;

        if (!int.TryParse(sp.ReadLine(), out int data))
        {
            Debug.LogError("Received Data is not an integer");
            return;
        }

        Debug.Log($"Sending Data: {data}");
        dataReceived.Invoke(data);
    }

    private void Update()
    {
        // Debug.Log($"Receive: {_serialPort.ReadLine()}");
    }

    private void OnDestroy()
    {
        if (_serialPort.IsOpen)
            _serialPort.Close();
    }
}
