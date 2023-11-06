using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Artist
{
    public class UpdateArtistFollowerCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string ArtistId { get; set; }
        public string MemberId { get; set; }
        public bool StartFollowing { get; set; }
    }
}
