using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IEventTypeService
    {
        Task<ServiceResponse<List<EventTypeDto>>> GetEventTypes(CancellationToken cancellationToken);
        Task<ServiceResponse<List<EventTypeDto>>> SearchEventType(string name, CancellationToken cancellationToken);
    }
}
