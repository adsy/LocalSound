using localsound.backend.api.Commands.Artist;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace localsound.backend.api.Controllers
{
    [Route("api/artist")]
    [ApiController]
    public class ArtistController : BaseApiController
    {
        [HttpPut]
        [Route("{memberId}")]
        public async Task<ActionResult> UpdateArtistDetails([FromBody] UpdateArtistDto updateArtistDto, string memberId)
        {
            var result = await Mediator.Send(new UpdateArtistDetailsCommand
            {
                UserId = AppUserId,
                MemberId = memberId, 
                UpdateArtistDto = updateArtistDto
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }
    }
}
