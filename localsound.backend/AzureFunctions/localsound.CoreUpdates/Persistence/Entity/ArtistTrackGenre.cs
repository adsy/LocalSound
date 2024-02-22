using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistTrackGenre
    {
        [ForeignKey("ArtistTrack")]
        public int ArtistTrackId { get; set; }
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }

        public ArtistTrack ArtistTrack { get; set; }
    }
}
