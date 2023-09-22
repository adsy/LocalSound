using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class AddTrackSupportingDetailsCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public Guid TrackId { get; set; }
        public TrackUploadDto TrackData { get; set; }
    }
}
