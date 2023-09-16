using AutoMapper;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly ILogger<EventTypeService> _logger;
        private readonly IMapper _mapper;

        public EventTypeService(IMapper mapper, ILogger<EventTypeService> logger, IEventTypeRepository eventTypeRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _eventTypeRepository = eventTypeRepository;
        }

        public async Task<ServiceResponse<List<EventTypeDto>>> SearchEventType(string name, CancellationToken cancellationToken)
        {
            try
            {
                var eventTypeResult = await _eventTypeRepository.SearchEventTypeAsync(name, cancellationToken);

                if (!eventTypeResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse<List<EventTypeDto>>(eventTypeResult.StatusCode);
                }

                var returnList = _mapper.Map<List<EventTypeDto>>(eventTypeResult.ReturnData);

                return new ServiceResponse<List<EventTypeDto>>(HttpStatusCode.OK)
                {
                    ReturnData = returnList
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(GenreService)} - {nameof(SearchEventType)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<EventTypeDto>>(HttpStatusCode.InternalServerError, "There was an error while searching for the event type.");
            }
        }
    }
}
