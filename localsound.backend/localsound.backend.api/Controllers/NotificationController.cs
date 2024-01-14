using localsound.backend.api.Commands.Notification;
using localsound.backend.api.Queries.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace localsound.backend.api.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        [HttpGet]
        [Route("member/{memberId}/get-more-notifications")]
        public async Task<ActionResult> GetMoreNotifications([FromQuery] int page, string memberId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetMoreNotificationsQuery
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                Page = page
            }, cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpPut]
        [Route("member/{memberId}/notification/{notificationId}/click-notification")]
        public async Task<ActionResult> ClickNotification(string memberId, Guid notificationId)
        {
            var result = await Mediator.Send(new ClickNotificationCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                NotificationId = notificationId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }
    }
}
