using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackAudioFileContent
    {
        public Guid FileContentId { get; set; }
        [ForeignKey("ArtistTrack")]
        public int ArtistTrackId { get; set; }
        public string FileLocation { get; set; }
        public string FileExtensionType { get; set; }

        public virtual ArtistTrack ArtistTrack { get; set; }
    }
}
