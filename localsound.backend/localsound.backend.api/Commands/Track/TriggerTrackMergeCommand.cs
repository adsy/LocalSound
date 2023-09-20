using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Track
{
    public class TriggerTrackMergeCommand : IRequest<ServiceResponse>
    {
        public Guid PartialTrackId { get; set; }
        public Guid TrackId { get; set; }
    }
}
