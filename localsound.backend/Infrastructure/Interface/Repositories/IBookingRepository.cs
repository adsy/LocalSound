using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IBookingRepository
    {
        Task<ServiceResponse> CreateBookingAsync(Guid appUserId, CreateBookingDto bookingDto);
        Task<ServiceResponse<List<ArtistBooking>>> GetFutureBookingsAsync(Guid appUserId, bool? bookingConfirmed, int page);
    }
}
