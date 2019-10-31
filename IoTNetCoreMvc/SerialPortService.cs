using IoTNetCoreMvc.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO.Ports;

namespace IoTNetCoreMvc
{
    public class SerialPortService
    {
        private readonly IHubContext<ClientHub, IClientHub> _clientHub;
        private volatile int _value = 0;

        private static SerialPort _serialPort;
        private static object _lock = new object();

        public SerialPortService(IHubContext<ClientHub, IClientHub> clientHub)
        {
            _clientHub = clientHub;
            _serialPort = new SerialPort("/dev/ttyACM0") {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };
        }

        public void Start()
        {
            try
            {
                Console.WriteLine("Starting Arduino Sevice...");
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start Arduino Service Fail: " + ex.Message);
            }
        }

        public void Stop()
        {
            if (_serialPort.IsOpen == true)
            {
                _serialPort.Close();
            }
        }

        public static void WriteCommand(string command)
        {
            try
            {
                _serialPort.Write(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error WriteCommand: " + ex.ToString());
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var serialPort = (SerialPort)sender;
                var serialdata = serialPort.ReadExisting();

                if (int.TryParse(serialdata, out int result))
                {
                    if (_value != result)
                    {
                        _value = result;
                        _clientHub.Clients.All.RecebeEstadoServo(_value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataReceived Arduino Service Fail: " + ex.Message);
            }
        }
    }
}
