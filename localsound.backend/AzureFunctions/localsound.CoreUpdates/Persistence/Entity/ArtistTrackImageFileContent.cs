using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistTrackImageFileContent
    {
        public Guid FileContentId { get; set; }
        [ForeignKey("ArtistTrackImage")]
        public int ArtistTrackImageId { get; set; }
        public string FileLocation { get; set; }
        public string FileExtensionType { get; set; }

        public virtual ArtistTrackImage ArtistTrackImage { get; set; }
    }
}
