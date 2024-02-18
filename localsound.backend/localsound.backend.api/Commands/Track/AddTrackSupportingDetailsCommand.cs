using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class AddTrackSupportingDetailsCommand : IRequest<ServiceResponse<int>>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public TrackUploadDto TrackData { get; set; }
    }
}
