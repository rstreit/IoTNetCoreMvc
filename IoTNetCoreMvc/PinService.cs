using IoTNetCoreMvc.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace IoTNetCoreMvc
{
    public class PinService
    {
        public static volatile bool Led = false;
        public static volatile bool Button = false;

        private static volatile bool lastButton = false;
        private static int pinButton = 23;
        private static int pinLed = 24;

        private readonly IHubContext<ClientHub, IClientHub> _clientHub;

        public PinService(IHubContext<ClientHub, IClientHub> clientHub)
        {
            _clientHub = clientHub;
        }

        public void Start()
        {
            Init();
        }

        public void Stop()
        {
        }

        public static void EstadoLed(bool ligado)
        {
            try
            {
                Pi.Gpio[pinLed].Write(ligado);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EstadoLed: " + ex.Message);
            }
        }

        private void Init()
        {
            try
            {
                Pi.Init<BootstrapWiringPi>();
                Console.WriteLine(Pi.Info);

                var ReadPinButton = Pi.Gpio[pinButton];
                ReadPinButton.PinMode = GpioPinDriveMode.Input;

                var ReadPinLed = Pi.Gpio[pinLed];
                ReadPinLed.PinMode = GpioPinDriveMode.Output;

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Button = Pi.Gpio[pinButton].Read();

                            if (Button != lastButton)
                            {
                                lastButton = Button;
                                _clientHub.Clients.All.RecebeEstadoLed(lastButton).Wait();
                            }

                            Thread.Sleep(100);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Task EstadoLed: " + ex.Message);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Init rasp service fails: " + ex.Message);
            }
        }
    }
}

