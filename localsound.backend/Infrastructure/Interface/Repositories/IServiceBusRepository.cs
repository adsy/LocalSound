using localsound.backend.Domain.Model;
using LocalSound.Shared.Package.ServiceBus.Dto.Enum;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IServiceBusRepository
    {
        Task<ServiceResponse> SendDeleteQueueEntry<T>(T entity);
    }
}
