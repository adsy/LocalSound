using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class UpdateTrackSupportingDetailsCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public Guid TrackId { get; set; }
        public TrackUpdateDto TrackData { get; set; }
    }
}
