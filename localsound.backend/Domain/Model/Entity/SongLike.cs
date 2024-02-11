using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class SongLike
    {
        [ForeignKey("ArtistTrackUpload")]
        public Guid ArtistTrackId { get; set; }
        [ForeignKey("Account")]
        public Guid AppUserId { get; set; }

        public virtual ArtistTrackUpload ArtistTrackUpload { get; set; }
        public virtual Account Account { get; set; }
    }
}
