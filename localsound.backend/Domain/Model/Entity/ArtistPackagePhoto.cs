using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistPackagePhoto
    {
        public Guid ArtistPackagePhotoId { get; set; }
        [ForeignKey("ArtistPackage")]
        public Guid ArtistPackageId { get; set; }
        [ForeignKey("FileContent")]
        public Guid FileContentId { get; set; }
        public string PhotoUrl { get; set; }

        public virtual ArtistPackage ArtistPackage { get; set; }
        public virtual FileContent FileContent { get; set; }
    }
}
