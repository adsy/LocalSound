using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface INotificationService
    {
        Task<ServiceResponse<NotificationCreatedResponseDto>> CreateNotification(Guid creatorUserId, string receiverMemberId, string message, string redirectUrl);
        Task<ServiceResponse> DeleteUserNotification(Guid userId, string memberId, Guid notificationId);
        Task<ServiceResponse<NotificationListResponseDto>> GetMoreUserNotifications(Guid userId, string memberId);
        Task<ServiceResponse<NotificationListResponseDto>> GetUserNotifications(Guid userId, CancellationToken cancellationToken);
    }
}
