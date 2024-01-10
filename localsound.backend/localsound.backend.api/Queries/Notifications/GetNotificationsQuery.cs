using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.Notifications
{
    public class GetNotificationsQuery : IRequest<ServiceResponse<List<NotificationDto>>>
    {
        public Guid UserId { get; set; }
    }
}
