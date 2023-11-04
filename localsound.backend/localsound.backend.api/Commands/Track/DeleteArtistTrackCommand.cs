using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class DeleteArtistTrackCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get;set; }
        public Guid TrackId { get; set; }
    }
}
