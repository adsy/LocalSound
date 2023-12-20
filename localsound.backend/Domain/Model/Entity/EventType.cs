namespace localsound.backend.Domain.Model.Entity
{
    public class EventType
    {
        public Guid EventTypeId { get; set; }
        public string EventTypeName { get; set; }

        public virtual ICollection<ArtistBooking> RelatedBookings { get; set; }
    }
}
