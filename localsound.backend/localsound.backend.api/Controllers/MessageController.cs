using localsound.backend.api.Commands.Message;
using localsound.backend.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/message")]
    public class MessageController:BaseApiController
    {
        [Route("member/{memberId}/dismiss-message/{message}")]
        [HttpPost]
        public async Task<ActionResult> DismissOnboardingMessage(string memberId, MessageEnum message)
        {
            var result = await Mediator.Send(new DismissMessageCommand
            {
                UserId = CurrentUser.AppUserId, 
                MemberId = memberId, 
                Message = message
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }
    }
}
