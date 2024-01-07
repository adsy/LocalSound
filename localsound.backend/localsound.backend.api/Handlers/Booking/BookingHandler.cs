using localsound.backend.api.Commands.Bookings;
using localsound.backend.api.Queries.Bookings;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Booking
{
    public class BookingHandler : IRequestHandler<CreateBookingCommand, ServiceResponse>, 
        IRequestHandler<GetUserBookingsQuery, ServiceResponse<BookingListResponse>>,
        IRequestHandler<GetCompletedBookingsQuery, ServiceResponse<BookingListResponse>>,
        IRequestHandler<AcceptBookingCommand, ServiceResponse>,
        IRequestHandler<CancelBookingCommand, ServiceResponse>
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

        public async Task<ServiceResponse<BookingListResponse>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            return await _bookingService.GetNonCompletedBookings(request.AppUserId, request.MemberId, request.BookingConfirmed, request.Page);
        }

        public async Task<ServiceResponse> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            return await _bookingService.CancelBooking(request.AppUserId, request.MemberId, request.BookingId);
        }

        public async Task<ServiceResponse> Handle(AcceptBookingCommand request, CancellationToken cancellationToken)
        {
            return await _bookingService.AcceptBooking(request.AppUserId, request.MemberId, request.BookingId);
        }

        public async Task<ServiceResponse<BookingListResponse>> Handle(GetCompletedBookingsQuery request, CancellationToken cancellationToken)
        {
            return await _bookingService.GetCompletedBookings(request.AppUserId, request.MemberId, request.Page);
        }
    }
}
