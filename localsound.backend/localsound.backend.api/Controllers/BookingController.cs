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
        [Route("member/{memberId}/get-future-bookings")]
        public async Task<ActionResult> GetFutureBookings([FromQuery] bool? bookingConfirmed, [FromQuery] int page, string memberId)
        {
            var result = await Mediator.Send(new GetUserBookingsQuery
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                BookingConfirmed = bookingConfirmed,
                Page = page
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }
    }
}
