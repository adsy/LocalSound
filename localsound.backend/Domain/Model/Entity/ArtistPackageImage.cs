using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistPackageImage
    {
        public int ArtistPackageImageId { get; set; }
        [ForeignKey("ArtistPackage")]
        public Guid ArtistPackageId { get; set; }
        public string PhotoUrl { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual ArtistPackage ArtistPackage { get; set; }
        public virtual ArtistPackageImageFileContent ArtistPackageImageFileContent { get; set; }
    }
}
