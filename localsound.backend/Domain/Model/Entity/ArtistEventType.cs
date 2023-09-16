using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistEventType
    {
        [ForeignKey("EventType")]
        public Guid EventTypeId { get; set; }
        [ForeignKey("Artist")]
        public Guid AppUserId { get; set; }

        public virtual EventType EventType { get;set;}
        public virtual Artist Artist { get; set; }
    }
}
