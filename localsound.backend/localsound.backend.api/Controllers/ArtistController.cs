using localsound.backend.api.Commands.Artist;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class ArtistController : BaseApiController
    {
        [HttpPut]
        [Route("/artist/{memberId}")]
        public async Task<ActionResult> UpdateArtistDetails([FromBody] UpdateArtistDto updateArtistDto)
        {
            var result = await Mediator.Send(new UpdateArtistDetailsCommand
            {
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
