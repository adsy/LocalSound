using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackUpload
    {
        public int ArtistTrackUploadId { get; set; }
        [ForeignKey(nameof(Artist))]
        public Guid AppUserId { get; set; }
        [ForeignKey(nameof(FileContent))]
        public Guid FileContentId { get; set; }
        public string TrackName { get; set; }

        public virtual Artist Artist {get;set;} 
        public virtual FileContent FileContent { get;set;} 
    }
}
