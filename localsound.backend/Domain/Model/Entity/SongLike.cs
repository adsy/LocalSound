using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class SongLike
    {
        public int SongLikeId { get; set; }
        [ForeignKey("ArtistTrackUpload")]
        public int ArtistTrackId { get; set; }
        public string MemberId { get; set; }

        public virtual ArtistTrack ArtistTrack { get; set; }
        public virtual Account Account { get; set; }
    }
}
