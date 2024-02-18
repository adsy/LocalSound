using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistPackagePhoto
    {
        public int ArtistPackagePhotoId { get; set; }
        [ForeignKey("ArtistPackage")]
        public Guid ArtistPackageId { get; set; }
        [ForeignKey("FileContent")]
        public Guid FileContentId { get; set; }
        public string PhotoUrl { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual FileContent FileContent { get; set; }
    }
}
