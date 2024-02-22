using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistTrackImage
    {
        public int ArtistTrackImageId { get; set; }
        [ForeignKey(nameof(ArtistTrack))]
        public int ArtistTrackId { get; set; }
        public string? TrackImageUrl { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual ArtistTrack ArtistTrack { get; set; }
        public virtual ArtistTrackImageFileContent ArtistTrackImageFileContent { get; set; }
    }
}
