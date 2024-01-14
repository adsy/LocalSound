using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface INotificationRepository
    {
        Task<ServiceResponse<Notification>> CreateNotificationAsync(Guid creatorUserId, Guid receiverUserId, string message, string redirectUrl);
        Task<ServiceResponse<List<Notification>>> GetUserNotificationsAsync(Guid userId, int page);
        Task<ServiceResponse<Notification>> GetUserNotificationAsync(Guid notificationId);
    }
}
