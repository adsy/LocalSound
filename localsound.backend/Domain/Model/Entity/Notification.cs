using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        [ForeignKey("NotificationReceiver")]
        public Guid NotificationReceiverId { get; set; }
        [ForeignKey("NotificationCreator")]
        public Guid NotificationCreatorId { get; set; }
        public string NotificationMessage { get; set; }
        public string RedirectUrl { get; set; }
        public bool NotificationViewed { get; set; }

        public virtual AppUser NotificationReceiver { get; set; }
        public virtual AppUser NotificationCreator { get; set; }
    }
}
