using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class LikeArtistTrackCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; } 
        public string MemberId { get; set; } 
        public int TrackId { get; set; }
        public string ArtistMemberId { get; set; }
    }
}
