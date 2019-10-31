using IoTNetCoreMvc.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTNetCoreMvc
{
    internal class MyBackgroundService : IHostedService, IDisposable
    {
        private readonly SerialPortService _serialPortService;
        private readonly PinService _pinService;

        public MyBackgroundService(IHubContext<ClientHub, IClientHub> clientHub)
        {
            _serialPortService = new SerialPortService(clientHub);
            _pinService = new PinService(clientHub);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serialPortService.Start();
            _pinService.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _serialPortService.Stop();
            _pinService.Stop();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
