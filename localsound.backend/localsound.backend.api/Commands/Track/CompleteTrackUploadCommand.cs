using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class CompleteTrackUploadCommand : IRequest<ServiceResponse>
    {
        public string MemberId { get; set; }
        public Guid AppUserId { get; set; }
        public Guid PartialTrackId { get; set; }
        public TrackUploadDto FormData { get; set; }
    }
}
