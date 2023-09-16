using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        private readonly LocalSoundDbContext _dbContext;

        public EventTypeRepository(LocalSoundDbContext dbContext)
        {
            _dbContext = dbContext;
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

                return new ServiceResponse<List<EventType>>(HttpStatusCode.InternalServerError, "There was an error while searching for the genre.");
            }
        }
    }
}
