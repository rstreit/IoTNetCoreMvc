using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IoTNetCoreMvc.Hubs
{
    public class ClientHub : Hub<IClientHub>
    {
        IHubContext<ClientHub, IClientHub> _clientHub;

        public ClientHub(IHubContext<ClientHub, IClientHub> clientHub)
        {
            _clientHub = clientHub;
        }

        public async Task EstadoBotao(bool ligado)
        {
            PinService.EstadoLed(ligado);
            await Task.FromResult(0);
        }

        public async Task EstadoSlider(int angulo)
        {
            SerialPortService.WriteCommand(angulo.ToString());
            await Task.FromResult(0);
        }
    }
}
