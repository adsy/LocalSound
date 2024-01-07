using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Queries.Bookings
{
    public class GetCompletedBookingsQuery : IRequest<ServiceResponse<List<BookingDto>>>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public int Page { get; set; }
    }
}
