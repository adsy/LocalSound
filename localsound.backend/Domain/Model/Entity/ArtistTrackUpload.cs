using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackUpload
    {
        public Guid ArtistTrackUploadId { get; set; }
        [ForeignKey(nameof(Artist))]
        public Guid AppUserId { get; set; }
        [ForeignKey("TrackData")]
        public Guid TrackDataId { get; set; }
        [ForeignKey("TrackImage")]
        public Guid? TrackImageId { get; set; }
        public string TrackName { get; set; }
        public string TrackDescription { get; set; }
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }
        public bool TrackReady { get; set; } = false;

        public virtual Artist Artist {get;set;}
        public Genre Genre { get; set;}
        public virtual FileContent TrackData { get;set;} 
        public virtual FileContent TrackImage { get;set; }
    }
}
