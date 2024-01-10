using localsound.backend.api.Queries.Notifications;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Notification
{
    public class NotificationHandler : IRequestHandler<GetNotificationsQuery, ServiceResponse<List<NotificationDto>>>
    {
        private readonly INotificationService _notificationService;

        public async Task<ServiceResponse<List<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationService.GetUserNotifications(request.UserId, cancellationToken);
        }
    }
}
