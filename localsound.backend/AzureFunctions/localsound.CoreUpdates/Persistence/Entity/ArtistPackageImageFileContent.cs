using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistPackageImageFileContent
    {
        public Guid FileContentId { get; set; }
        [ForeignKey("ArtistPackageImage")]
        public int AristPackagePhotoId { get; set; }
        public string FileLocation { get; set; }
        public string FileExtensionType { get; set; }

        public virtual ArtistPackageImage ArtistPackageImage { get; set; }
    }
}
