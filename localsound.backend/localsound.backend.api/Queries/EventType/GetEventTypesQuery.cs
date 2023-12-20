using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.EventType
{
    public class GetEventTypesQuery : IRequest<ServiceResponse<List<EventTypeDto>>>
    {
    }
}
