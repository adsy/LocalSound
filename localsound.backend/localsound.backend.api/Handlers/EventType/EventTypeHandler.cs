using localsound.backend.api.Queries;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.EventType
{
    public class EventTypeHandler : IRequestHandler<SearchEventTypeQuery, ServiceResponse<List<EventTypeDto>>>
    {
        private readonly IEventTypeService _eventTypeService;

        public EventTypeHandler(IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
        }

        public async Task<ServiceResponse<List<EventTypeDto>>> Handle(SearchEventTypeQuery request, CancellationToken cancellationToken)
        {
            return await _eventTypeService.SearchEventType(request.Name, cancellationToken);
        }
    }
}
