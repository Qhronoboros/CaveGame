// Source: https://www.alanzucconi.com/2016/12/01/asynchronous-serial-communication/

using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

// [RequireComponent(typeof(DataDistributer))]
public class SerialCommunication : MonoBehaviour
{
    // private DataDistributer _dataDistributer;
    [SerializeField] private List<DataDistributer> _dataDistributers;

    [SerializeField] private string _portName;
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

        // _dataDistributer = GetComponent<DataDistributer>();

        // StartSerialCommunication();
    }

    public void SetPortName(string name) => _portName = name;

    public void StartSerialCommunication()
    {
        if (!IsLooping())
        {
            _thread = new Thread(ThreadLoop);
            _thread.Start();
        }
    }

    public void StopThread() { lock (_lock) { _isLooping = false; } }

    public void RestartThread()
    {
        StopThread();
        StartSerialCommunication();
    }

    private void ThreadLoop()
    {
        _isLooping = true;

        _serialPort = new SerialPort(_portName, 9600);

        _serialPort.Close();
        _serialPort.Open();

        while (IsLooping())
        {
            string stringData = _serialPort.ReadLine();

            // Debug.Log(stringData);

            // * Make sure the data being received has a - as a divider
            string[] data = stringData.Split('-');

            if (data.Length != _dataDistributers.Count) continue;

            for (int i = 0; i < _dataDistributers.Count; i++)
            {
                if (!int.TryParse(data[i], out int dataParsed))
                {
                    Debug.LogError($"Received Data {data[i]} is not an integer");
                    break;
                }

                _dataDistributers[i].data = dataParsed;
                _dataDistributers[i].hasNewData = true;
            }
        }

        if (_serialPort.IsOpen)
            _serialPort.Close();
    }

    private bool IsLooping() { lock (_lock) { return _isLooping; } }

    private void OnEnable() => StartSerialCommunication();
    private void OnDisable() => StopThread();
}
