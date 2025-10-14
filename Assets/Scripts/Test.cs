using UnityEngine;
using System.IO.Ports;

public class Test : MonoBehaviour
{
    private SerialPort mySerialPort;
    private SerialDataReceivedEventHandler dataReceivedHandler;

    private void Start()
    {
        mySerialPort = new SerialPort("COM5");

        mySerialPort.BaudRate = 9600;
        mySerialPort.Parity = Parity.None;
        mySerialPort.StopBits = StopBits.One;
        mySerialPort.DataBits = 8;
        mySerialPort.Handshake = Handshake.None;
        mySerialPort.RtsEnable = true;

        mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

        mySerialPort.Open();
    }

    private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
    {
        var sp = (SerialPort)sender;
        var indata = sp.ReadLine();
        Debug.Log(indata);
    }

    private void OnDestroy()
    {
        if (mySerialPort.IsOpen)
            mySerialPort.Close();

        // mySerialPort.DataReceived -= dataReceivedHandler;
    }
}
