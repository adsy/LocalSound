using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackGenre
    {
        [ForeignKey("ArtistTrack")]
        public int ArtistTrackId { get; set; }
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }

        public ArtistTrack ArtistTrack { get; set; }
        public Genre Genre { get; set; }
    }
}
