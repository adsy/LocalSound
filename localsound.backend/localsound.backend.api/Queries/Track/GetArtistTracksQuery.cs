using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.Track
{
    public class GetArtistTracksQuery : IRequest<ServiceResponse<List<ArtistTrackUploadDto>>>
    {
        public string MemberId { get; set; }
    }
}
