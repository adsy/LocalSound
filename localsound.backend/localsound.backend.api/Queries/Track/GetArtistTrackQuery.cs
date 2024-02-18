using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.Track
{
    public class GetArtistTrackQuery : IRequest<ServiceResponse<ArtistTrackUploadDto>>
    {
        public string MemberId { get; set; }
        public int TrackId { get; set; }
    }
}
