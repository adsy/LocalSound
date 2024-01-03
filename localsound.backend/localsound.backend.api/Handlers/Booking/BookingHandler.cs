using localsound.backend.api.Commands.Bookings;
using localsound.backend.api.Queries.Bookings;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Booking
{
    public class BookingHandler : IRequestHandler<CreateBookingCommand, ServiceResponse>, 
        IRequestHandler<GetUserBookingsQuery, ServiceResponse<List<BookingDto>>>
    {
        private readonly IBookingService _bookingService;

        public BookingHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<ServiceResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            return await _bookingService.CreateBooking(request.AppUserId, request.MemberId, request.BookingDto, cancellationToken);
        }

        public async Task<ServiceResponse<List<BookingDto>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            return await _bookingService.GetFutureBookings(request.AppUserId, request.MemberId, request.BookingConfirmed, request.Page);
        }
    }
}
