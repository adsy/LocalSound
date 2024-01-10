using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface INotificationService
    {
        Task<ServiceResponse<NotificationCreatedResponseDto>> CreateNotification(Guid creatorUserId, string receiverMemberId, string message, string redirectUrl);
        Task<ServiceResponse<List<NotificationDto>>> GetUserNotifications(Guid userId, CancellationToken cancellationToken);
    }
}
