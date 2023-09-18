using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistEquipment
    {
        [ForeignKey("Artist")]
        public Guid AppUserId { get; set; }
        public Guid EquipmentId { get; set; }
        public string EquipmentName { get; set; }

        public Artist Artist { get; set; }
    }
}
