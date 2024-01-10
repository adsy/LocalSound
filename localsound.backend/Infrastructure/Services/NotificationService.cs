using AutoMapper;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<NotificationDto>>> GetUserNotifications(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, 0);

                if (!notifications.IsSuccessStatusCode || notifications.ReturnData == null)
                {
                    var itemAddErrorMessage = $"{nameof(NotificationService)} - {nameof(GetUserNotifications)} - " +
                        $"Error occured retrieving notifications for user:{userId}";
                    _logger.LogError(itemAddErrorMessage);

                    return new ServiceResponse<List<NotificationDto>>(HttpStatusCode.InternalServerError);
                }

                var notificationList = _mapper.Map<List<NotificationDto>>(notifications.ReturnData);
                foreach(var notification in notificationList)
                {

                }

                return new ServiceResponse<List<NotificationDto>>(HttpStatusCode.OK)
                {
                    ReturnData = notificationList
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(NotificationService)} - {nameof(GetUserNotifications)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<NotificationDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting user notifications"
                };
            }
        }
    }
}
