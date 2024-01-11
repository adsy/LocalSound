using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Dto.Response
{
    public class NotificationListResponseDto
    {
        public List<NotificationDto> NotificationList { get; set; }
        public bool CanLoadMore { get; set; }
    }
}
