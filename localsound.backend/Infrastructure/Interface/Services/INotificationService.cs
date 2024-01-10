using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface INotificationService
    {
        Task<ServiceResponse<List<NotificationDto>>> GetUserNotifications(Guid userId, CancellationToken cancellationToken);
    }
}
