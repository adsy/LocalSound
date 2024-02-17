using localsound.backend.api.Commands.Bookings;
using localsound.backend.api.Queries.Bookings;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : BaseApiController
    {
        [HttpPost]
        [Route("member/{memberId}/create-booking")]
        public async Task<ActionResult> CreateBooking([FromBody] CreateBookingDto bookingDto, string memberId)
        {
            var result = await Mediator.Send(new CreateBookingCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                BookingDto = bookingDto,
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpGet]
        [Route("member/{memberId}/get-bookings")]
        public async Task<ActionResult> GetNonCompletedBookings([FromQuery] bool? bookingConfirmed, [FromQuery] int? lastBookingId, string memberId)
        {
            var result = await Mediator.Send(new GetUserBookingsQuery
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                BookingConfirmed = bookingConfirmed,
                LastBookingId = lastBookingId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpGet]
        [Route("member/{memberId}/get-completed-bookings")]
        public async Task<ActionResult> GetCompletedBookings([FromQuery] int? lastBookingId, string memberId)
        {
            var result = await Mediator.Send(new GetCompletedBookingsQuery
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                LastBookingId = lastBookingId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpPut]
        [Route("member/{memberId}/booking/{bookingId}/accept-booking")]
        public async Task<ActionResult> AcceptBooking(string memberId, int bookingId) 
        {
            var result = await Mediator.Send(new AcceptBookingCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                BookingId = bookingId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpPut]
        [Route("member/{memberId}/booking/{bookingId}/cancel-booking")]
        public async Task<ActionResult> CancelBooking(string memberId, int bookingId)
        {
            var result = await Mediator.Send(new CancelBookingCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                BookingId = bookingId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }
    }
}
