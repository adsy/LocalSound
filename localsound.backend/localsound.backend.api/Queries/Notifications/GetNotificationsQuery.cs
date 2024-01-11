using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Notifications
{
    public class GetNotificationsQuery : IRequest<ServiceResponse<NotificationListResponseDto>>
    {
        public Guid UserId { get; set; }
    }
}
