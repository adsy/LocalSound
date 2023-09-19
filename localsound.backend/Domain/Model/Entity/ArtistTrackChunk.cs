using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistTrackChunk
    {
        public int ChunkId { get; set; }
        public Guid PartialTrackId { get; set; }
        public Guid AppUserId { get; set; }
        [ForeignKey("FileContent")]
        public Guid FileContentId { get; set; }

        public FileContent FileContent { get; set; }
    }
}
