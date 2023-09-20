namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistTrackChunkDto
    {

        public int ChunkId { get; set; }
        public Guid PartialTrackId { get; set; }
        public Guid AppUserId { get; set; }
        public Guid FileContentId { get; set; }
        public string FileLocation { get; set; }
        public Stream Data { get; set; }
    }
}
