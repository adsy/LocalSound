using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistEquipment
    {
        [ForeignKey("Artist")]
        public Guid AppUserId { get; set; }
        [ForeignKey("Equipment")]
        public Guid EquipmentId { get; set; }

        public Artist Artist { get; set; }
        public Equipment Equipment { get; set; }
    }
}
