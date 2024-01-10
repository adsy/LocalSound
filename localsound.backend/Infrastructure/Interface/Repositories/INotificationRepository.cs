using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface INotificationRepository
    {
        Task<ServiceResponse<List<Notification>>> GetUserNotificationsAsync(Guid userId, int page);
    }
}
