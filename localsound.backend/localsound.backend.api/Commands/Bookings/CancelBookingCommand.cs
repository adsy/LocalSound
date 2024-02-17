using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Bookings
{
    public class CancelBookingCommand : IRequest<ServiceResponse>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public int BookingId { get; set; }
    }
}
