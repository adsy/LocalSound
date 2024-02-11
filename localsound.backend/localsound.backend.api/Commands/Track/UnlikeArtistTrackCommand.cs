using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class UnlikeArtistTrackCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public Guid TrackId { get; set; }
    }
}
