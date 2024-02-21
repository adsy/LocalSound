using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackAudioFileContent
    {
        public Guid FileContentId { get; set; }
        [ForeignKey("ArtistTrackUpload")]
        public int ArtistTrackUploadId { get; set; }
        public string FileLocation { get; set; }
        public string FileExtensionType { get; set; }

        public virtual ArtistTrackUpload ArtistTrackUpload { get; set; }
    }
}
