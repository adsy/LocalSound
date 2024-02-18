using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Notification
{
    public class ClickNotificationCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public int NotificationId { get; set; }
    }
}
