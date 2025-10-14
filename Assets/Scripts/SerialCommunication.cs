// Source: https://www.alanzucconi.com/2016/12/01/asynchronous-serial-communication/

using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;
using System.Threading;

public class SerialCommunication : MonoBehaviour
{
    public UnityEvent<int> dataReceived;
    
    private SerialPort _serialPort;
    private Thread _thread;
    private readonly object _lock = new();
    private bool _isLooping;

    private void Awake()
    {
        if (GameManager.serialCommunication == null)
			GameManager.serialCommunication = this;
		else 
		{
			Debug.LogError($"A SerialCommunication already exists, deleting self: {name}");
			Destroy(gameObject);
		}

        StartSerialCommunication();
    }

    public void StartSerialCommunication()
    {
        if (!IsLooping())
        {
            _thread = new Thread(ThreadLoop);
            _thread.Start();
        }
    }

    public void StopThread() { lock (_lock) { _isLooping = false; } }

    private void ThreadLoop()
    {
        _serialPort = new SerialPort("COM5", 9600);
        _serialPort.Open();

        _isLooping = true;

        while (IsLooping())
        {
            string data = _serialPort.ReadLine();

            if (!int.TryParse(data, out int dataParsed))
            {
                Debug.LogError($"Received Data {data} is not an integer");
                continue;
            }

            Debug.Log($"Receiving Data: {dataParsed}");
            dataReceived.Invoke(dataParsed);
        }

        if (_serialPort.IsOpen)
            _serialPort.Close();
    }
    
    private bool IsLooping() { lock (_lock) { return _isLooping; } }

    private void OnDestroy() => StopThread();
}
