using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Commands.Notification
{
    public class CreateNotificationCommand : IRequest<ServiceResponse<NotificationCreatedResponseDto>>
    {
        public Guid CreatorUserId { get; set; }
        public string ReceiverMemberId { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }
}
