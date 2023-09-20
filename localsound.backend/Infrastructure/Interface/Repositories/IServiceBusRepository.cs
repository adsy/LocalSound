using localsound.backend.Domain.Model;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IServiceBusRepository
    {
        Task<ServiceResponse> PushMessageToQueueAsync(string queueName, string message);
    }
}
