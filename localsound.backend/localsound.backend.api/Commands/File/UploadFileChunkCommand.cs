using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.File
{
    public class UploadFileChunkCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public Guid PartialTrackId { get; set; }
        public IFormFile File { get; set; }
        public int ChunkId { get; set; }
    }
}
