using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Bookings
{
    public class AcceptBookingCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public Guid BookingId { get; set; }
    }
}
