using localsound.backend.api.Commands.Notification;
using localsound.backend.api.Queries.Notifications;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Notification
{
    public class NotificationHandler : IRequestHandler<GetNotificationsQuery, ServiceResponse<List<NotificationDto>>>,
        IRequestHandler<CreateNotificationCommand, ServiceResponse<NotificationCreatedResponseDto>>
    {
        private readonly INotificationService _notificationService;

        public NotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<ServiceResponse<List<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationService.GetUserNotifications(request.UserId, cancellationToken);
        }

        public async Task<ServiceResponse<NotificationCreatedResponseDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            return await _notificationService.CreateNotification(request.CreatorUserId, request.ReceiverMemberId, request.Message, request.RedirectUrl);
        }
    }
}
