﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Bookings
{
    public class GetUserBookingsQuery : IRequest<ServiceResponse<BookingListResponse>>
    {
        public Guid AppUserId { get; set; }
        public string MemberId { get; set; }
        public bool? BookingConfirmed { get; set; }
        public int? LastBookingId { get; set; }
    }
}
