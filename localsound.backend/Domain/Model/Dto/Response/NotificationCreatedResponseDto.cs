using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Dto.Response
{
    public class NotificationCreatedResponseDto
    {
        public Guid ReceiverUserId { get; set; }
        public NotificationDto Notification { get; set; }
    }
}
