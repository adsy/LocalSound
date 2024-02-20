using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Notifications
{
    public class GetMoreNotificationsQuery : IRequest<ServiceResponse<NotificationListResponseDto>>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public int? LastNotificationId { get; set; }
    }
}
