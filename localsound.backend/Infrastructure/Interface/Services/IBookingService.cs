using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IBookingService
    {
        Task<ServiceResponse> AcceptBooking(Guid appUserId, string memberId, Guid bookingId);
        Task<ServiceResponse> CancelBooking(Guid appUserId, string memberId, Guid bookingId);
        Task<ServiceResponse> CreateBooking(Guid appUserId, string memberId, CreateBookingDto bookingDto, CancellationToken cancellationToken);
        Task<ServiceResponse<BookingListResponse>> GetCompletedBookings(Guid appUserId, string memberId, int page);
        Task<ServiceResponse<BookingListResponse>> GetNonCompletedBookings(Guid appUserId, string memberId, bool? bookingConfirmed, int page);
    }
}
