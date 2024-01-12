using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Notification
{
    public class DeleteNotificationCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public Guid NotificationId { get; set; }
    }
}
