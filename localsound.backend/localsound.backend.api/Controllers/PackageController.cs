﻿using localsound.backend.api.Commands.ArtistPackages;
using localsound.backend.api.Queries.ArtistPackages;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackageController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("member/{memberId}")]
        public async Task<ActionResult> GetArtistPackages(string memberId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetArtistPackagesQuery
            {
                MemberId = memberId
            }, cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpPost]
        [Route("member/{memberId}")]
        public async Task<ActionResult> CreateArtistPackage([FromForm] ArtistPackageSubmissionDto createPackageDto, string memberId)
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

        [HttpDelete]
        [Route("member/{memberId}/package/{packageId}")]
        public async Task<ActionResult> DeleteArtistPackage(string memberId, Guid packageId)
        {
            var result = await Mediator.Send(new DeleteArtistPackageCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                PackageId = packageId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpPut]
        [Route("member/{memberId}/package/{packageId}")]
        public async Task<ActionResult> UpdateArtistPackage([FromForm] ArtistPackageSubmissionDto createPackageDto, string memberId, Guid packageId)
        {
            var result = await Mediator.Send(new UpdateArtistPackageCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                PackageDto = createPackageDto,
                PackageId = packageId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }
    }
}
