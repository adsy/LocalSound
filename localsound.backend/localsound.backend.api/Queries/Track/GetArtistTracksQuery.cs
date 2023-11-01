using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Track
{
    public class GetArtistTracksQuery : IRequest<ServiceResponse<TrackListResponseDto>>
    {
        public string MemberId { get; set; }
        public int Page { get; set; }
    }
}
