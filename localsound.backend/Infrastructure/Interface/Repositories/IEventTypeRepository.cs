using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IEventTypeRepository
    {
        Task<ServiceResponse<List<EventType>>> SearchEventTypeAsync(string name, CancellationToken cancellationToken);
    }
}
