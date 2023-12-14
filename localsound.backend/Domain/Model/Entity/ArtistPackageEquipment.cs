using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistPackageEquipment
    {
        public Guid ArtistPackageEquipmentId { get; set; }
        [ForeignKey("ArtistPackage")]
        public Guid ArtistPackageId { get; set; }
        public string EquipmentName { get; set; }

        public ArtistPackage ArtistPackage { get; set; }
    }
}
