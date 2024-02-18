using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class Notification
    {
        public int NotificationId { get; set; }
        [ForeignKey("NotificationReceiver")]
        public Guid NotificationReceiverId { get; set; }
        [ForeignKey("NotificationCreator")]
        public Guid NotificationCreatorId { get; set; }
        public string NotificationMessage { get; set; }
        public string RedirectUrl { get; set; }
        public bool NotificationViewed { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Account NotificationReceiver { get; set; }
        public virtual Account NotificationCreator { get; set; }
    }
}
