using localsound.backend.api.Commands.Notification;
using localsound.backend.api.Queries.Notifications;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Notification
{
    public class NotificationHandler : IRequestHandler<GetNotificationsQuery, ServiceResponse<NotificationListResponseDto>>,
        IRequestHandler<CreateNotificationCommand, ServiceResponse<NotificationCreatedResponseDto>>,
        IRequestHandler<GetMoreNotificationsQuery, ServiceResponse<NotificationListResponseDto>>,
        IRequestHandler<DeleteNotificationCommand, ServiceResponse>
    {
        private readonly INotificationService _notificationService;

        public NotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<ServiceResponse<NotificationListResponseDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationService.GetUserNotifications(request.UserId, cancellationToken);
        }

        public async Task<ServiceResponse<NotificationCreatedResponseDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            return await _notificationService.CreateNotification(request.CreatorUserId, request.ReceiverMemberId, request.Message, request.RedirectUrl);
        }

        public async Task<ServiceResponse<NotificationListResponseDto>> Handle(GetMoreNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationService.GetMoreUserNotifications(request.AppUserId, request.MemberId, request.Page);
        }

        public async Task<ServiceResponse> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            return await _notificationService.DeleteUserNotification(request.UserId, request.MemberId, request.NotificationId);
        }
    }
}
