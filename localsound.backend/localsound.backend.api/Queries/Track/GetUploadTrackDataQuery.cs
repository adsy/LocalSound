using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Track
{
    public class GetUploadTrackDataQuery : IRequest<ServiceResponse<TrackUploadSASDto>>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
    }
}
