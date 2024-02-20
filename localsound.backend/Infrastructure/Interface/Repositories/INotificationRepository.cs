using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface INotificationRepository
    {
        Task<ServiceResponse<Notification>> CreateNotificationAsync(Guid creatorUserId, Guid receiverUserId, string message, string redirectUrl);
        Task<ServiceResponse<List<Notification>>> GetUserNotificationsAsync(Guid userId, int? lastNotificationId);
        Task<ServiceResponse<Notification>> GetUserNotificationAsync(int notificationId);
        Task<ServiceResponse> ClickNotificationAsync(Guid userId, int notificationId);
        Task<ServiceResponse<int>> GetUnreadNotificationsCountAsync(Guid userId);
    }
}
