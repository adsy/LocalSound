using localsound.backend.api.Commands.Track;
using localsound.backend.api.Queries.Track;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.ModelAdaptor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/track")]
    [ApiController]
    public class TrackController : BaseApiController
    {
        private readonly BlobStorageSettingsAdaptor _blobStorageSettings;

        public TrackController(BlobStorageSettingsAdaptor blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings;
        }

        [HttpGet]
        [Route("member/{memberId}/upload-token")]
        public async Task<ActionResult> SASToken(string memberId)
        {
            var result = await Mediator.Send(new GetUploadTrackDataQuery
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok(result.ReturnData);
        }

        [HttpPost]
        [Route("member/{memberId}/track/{trackId}")]
        public async Task<ActionResult> UploadTrackSupportingDetails([FromForm] TrackUploadDto data, string memberId, Guid trackId)
        {
            var result = await Mediator.Send(new AddTrackSupportingDetailsCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = trackId,
                TrackData = data
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok();
        }

        [HttpGet]
        [Route("member/{memberId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetArtistTrackUploads(string memberId)
        {
            var result = await Mediator.Send(new GetArtistTracksQuery
            {
                MemberId = memberId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok(result.ReturnData);
        }
    }
}
