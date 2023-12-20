using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<EventTypeRepository> _logger;

        public EventTypeRepository(LocalSoundDbContext dbContext, ILogger<EventTypeRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<EventType>>> GetEventTypesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var eventTypes = await _dbContext.EventType.ToListAsync(cancellationToken);

                return new ServiceResponse<List<EventType>>(HttpStatusCode.OK)
                {
                    ReturnData = eventTypes
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(EventTypeRepository)} - {nameof(GetEventTypesAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<EventType>>(HttpStatusCode.InternalServerError, "There was an error getting the event types.");
            }
        }

        public async Task<ServiceResponse<List<EventType>>> SearchEventTypeAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                var genre = await _dbContext.EventType.Where(x => x.EventTypeName.Contains(name)).ToListAsync(cancellationToken);

                return new ServiceResponse<List<EventType>>(HttpStatusCode.OK)
                {
                    ReturnData = genre
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(EventTypeRepository)} - {nameof(SearchEventTypeAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<EventType>>(HttpStatusCode.InternalServerError, "There was an error while searching for the event type.");
            }
        }
    }
}
