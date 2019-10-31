using System.Threading.Tasks;

namespace IoTNetCoreMvc.Hubs
{
    public interface IClientHub
    {
        Task RecebeEstadoServo(int angulo);

        Task RecebeEstadoLed(bool ligado);
    }
}
