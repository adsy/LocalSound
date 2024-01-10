using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(LocalSoundDbContext dbContext, ILogger<NotificationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<Notification>>> GetUserNotificationsAsync(Guid userId, int page)
        {
            try
            {
                var notifications = await _dbContext.Notification.Where(x => x.NotificationReceiverId == userId)
                    .Skip(page * 10)
                    .Take(10)
                    .ToListAsync();

                return new ServiceResponse<List<Notification>>(HttpStatusCode.OK)
                {
                    ReturnData = notifications
                };
            }   
            catch(Exception e)
            {
                var message = $"{nameof(NotificationRepository)} - {nameof(GetUserNotificationsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<Notification>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting user notifications"
                };
            }
        }
    }
}
