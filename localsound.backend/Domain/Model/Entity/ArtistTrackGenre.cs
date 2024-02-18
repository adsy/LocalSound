using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackGenre
    {
        [ForeignKey("ArtistTrackUpload")]
        public int ArtistTrackUploadId { get; set; }
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }

        public ArtistTrackUpload ArtistTrackUpload { get; set; }
        public Genre Genre { get; set; }

    }
}
