using localsound.backend.Domain.Model;
using MediatR;
using localsound.backend.Domain.Model.Dto.Response;

namespace localsound.backend.api.Queries.Bookings
{
    public class GetCompletedBookingsQuery : IRequest<ServiceResponse<BookingListResponse>>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public int? LastBookingId { get; set; }
    }
}
