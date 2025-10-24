// Source: https://www.alanzucconi.com/2016/12/01/asynchronous-serial-communication/

using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;
using System.Threading;

[RequireComponent(typeof(DataDistributer))]
public class SerialCommunication : MonoBehaviour
{
    private DataDistributer _dataDistributer;

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

        _dataDistributer = GetComponent<DataDistributer>();

        StartSerialCommunication();
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

    private void ThreadLoop()
    {
        _isLooping = true;

        _serialPort = new SerialPort(_portName, 9600);
        _serialPort.Open();

        while (IsLooping())
        {
            string data = _serialPort.ReadLine();

            if (!int.TryParse(data, out int dataParsed))
            {
                Debug.LogError($"Received Data {data} is not an integer");
                continue;
            }

            _dataDistributer.data = dataParsed;
            _dataDistributer.hasNewData = true;
        }

        if (_serialPort.IsOpen)
            _serialPort.Close();
    }

    private bool IsLooping() { lock (_lock) { return _isLooping; } }

    private void OnDestroy() => StopThread();
}
