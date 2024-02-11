using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackLikeCount
    {
        [ForeignKey("ArtistTrackUpload")]
        public Guid ArtistTrackId { get; set; }
        public int LikeCount { get; set; }

        public virtual ArtistTrackUpload ArtistTrackUpload { get; set; }
    }
}
