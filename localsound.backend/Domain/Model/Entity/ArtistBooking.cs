using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistBooking
    {
        public Guid BookingId { get; set; }
        [ForeignKey("Artist")]
        public Guid ArtistId { get; set; }
        [ForeignKey("Booker")]
        public Guid BookerId { get; set; }
        [ForeignKey("Package")]
        public Guid PackageId { get; set; }
        [ForeignKey("EventType")]
        public Guid EventTypeId { get; set; }
        public string BookingDescription { get; set; }
        public string BookingAddress { get; set; }
        public decimal BookingLength { get; set; }
        public DateTime BookingDate { get; set; }
        public bool? BookingConfirmed { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual AppUser Booker { get; set; }
        public virtual ArtistPackage Package { get; set; }
        public virtual EventType EventType { get; set; }
    }
}
