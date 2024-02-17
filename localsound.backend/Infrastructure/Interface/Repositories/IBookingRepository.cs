using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IBookingRepository
    {
        Task<ServiceResponse> AcceptBookingAsync(Guid appUserId, int bookingId);
        Task<ServiceResponse> CancelBookingAsync(Guid appUserId, int bookingId, CustomerTypeEnum customerType);
        Task<ServiceResponse> CreateBookingAsync(Guid appUserId, CreateBookingDto bookingDto);
        Task<ServiceResponse<List<ArtistBooking>>> GetNonCompletedBookingsAsync(Guid appUserId, bool? bookingConfirmed, int? lastBookingId);
        Task<ServiceResponse<List<ArtistBooking>>> GetCompletedBookingsAsync(Guid appUserId, int? LastBookingId);
    }
}
