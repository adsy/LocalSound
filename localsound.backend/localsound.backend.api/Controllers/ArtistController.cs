using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class ArtistController : BaseApiController
    {
        [HttpPut]
        [Route("/artist/{memberId}")]
        public async Task<ActionResult> UpdateArtistDetails()
        {
            var result = await Mediator.Send(new UpdateArtistDetailsCommand());
        }
    }
}
