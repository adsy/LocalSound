using localsound.backend.api.Commands.Packages;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/package")]
    [ApiController]
    public class PackageController : BaseApiController
    {
        [HttpPost]
        [Route("member/{memberId}")]
        public async Task<ActionResult> CreateArtistPackage([FromForm] CreatePackageDto createPackageDto, string memberId)
        {
            var result = await Mediator.Send(new CreateArtistPackageCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                PackageDto = createPackageDto,
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }
    }
}
