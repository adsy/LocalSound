using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Track
{
    public class GetTracksQuery : IRequest<ServiceResponse<TrackListResponseDto>>
    {
        public Guid? UserId { get; set; }
        public string MemberId { get; set; }
        public int? LastTrackId { get; set; }
        public PlaylistTypeEnum PlaylistType { get; set; }
    }
}
