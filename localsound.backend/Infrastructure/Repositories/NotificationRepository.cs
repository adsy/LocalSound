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

        public async Task<ServiceResponse<Notification>> CreateNotificationAsync(Guid creatorUserId, Guid receiverUserId, string message, string redirectUrl)
        {
            try
            {
                var notification = await _dbContext.Notification.AddAsync(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationCreatorId = creatorUserId,
                    NotificationReceiverId = receiverUserId,
                    NotificationMessage = message,
                    CreatedOn = DateTime.Now,
                    RedirectUrl = redirectUrl
                });

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<Notification>(HttpStatusCode.OK)
                {
                    ReturnData = notification.Entity
                };
            }
            catch(Exception e)
            {
                var errorMessage = $"{nameof(NotificationRepository)} - {nameof(GetUserNotificationsAsync)} - {e.Message}";
                _logger.LogError(e, errorMessage);

                return new ServiceResponse<Notification>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteUserNotificationAsync(Guid userId, Guid notificationId)
        {
            try
            {
                var notification = await _dbContext.Notification.FirstOrDefaultAsync(x => x.NotificationReceiverId == userId && x.NotificationId == notificationId);

                if (notification == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound)
                    {
                        ServiceResponseMessage = "An error occured delete your notification, please try again..."
                    }; ;
                }

                _dbContext.Notification.Remove(notification);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var errorMessage = $"{nameof(NotificationRepository)} - {nameof(DeleteUserNotificationAsync)} - {e.Message}";
                _logger.LogError(e, errorMessage);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured delete your notification, please try again..."
                }; ;
            }
        }

        public async Task<ServiceResponse<Notification>> GetUserNotificationAsync(Guid notificationId)
        {
            try
            {
                var notification = await _dbContext.Notification
                    .Include(x => x.NotificationReceiver)
                    .Include(x => x.NotificationCreator)
                    .ThenInclude(x => x.Images)
                    .FirstOrDefaultAsync(x => x.NotificationId == notificationId);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<Notification>(HttpStatusCode.OK)
                {
                    ReturnData = notification
                };
            }
            catch (Exception e)
            {
                var errorMessage = $"{nameof(NotificationRepository)} - {nameof(GetUserNotificationAsync)} - {e.Message}";
                _logger.LogError(e, errorMessage);

                return new ServiceResponse<Notification>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<Notification>>> GetUserNotificationsAsync(Guid userId)
        {
            try
            {
                var notifications = await _dbContext.Notification
                    .Include(x => x.NotificationReceiver)
                    .Include(x => x.NotificationCreator)
                    .ThenInclude(x => x.Images)
                    .Where(x => x.NotificationReceiverId == userId)
                    .OrderByDescending(x => x.CreatedOn)
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
