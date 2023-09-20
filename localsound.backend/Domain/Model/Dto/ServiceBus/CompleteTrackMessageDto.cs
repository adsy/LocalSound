namespace localsound.backend.Domain.Model.Dto.ServiceBus
{
    public class CompleteTrackMessageDto
    {
        public Guid PartialTrackId { get; set; }
        public Guid ArtistTrackUploadId { get; set; }
    }
}
