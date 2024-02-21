using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
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
