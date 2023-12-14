using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistPackage
    {
        public Guid ArtistPackageId { get; set; }
        [ForeignKey("Artist")]
        public Guid AppUserId { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public string PackagePrice { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual ICollection<ArtistPackageEquipment> Equipment { get; set; }
        public virtual ICollection<ArtistPackagePhoto> PackagePhotos { get; set; }
    }
}
