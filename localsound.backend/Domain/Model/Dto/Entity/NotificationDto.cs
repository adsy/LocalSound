namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; }
        public string ReceivingMemberId { get; set; }
        public string CreatorMemberId { get; set; }
        public string NotificationMessage { get; set; }
        public string RedirectUrl { get; set; }
        public bool NotificationViewed { get; set; }
        public string UserImage { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
