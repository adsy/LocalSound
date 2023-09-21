using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using localsound.backend.api.Commands.Track;
using localsound.backend.api.Queries.Track;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.ModelAdaptor;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/upload-track")]
    [ApiController]
    public class UploadTrackController : BaseApiController
    {
        private readonly BlobStorageSettingsAdaptor _blobStorageSettings;

        public UploadTrackController(BlobStorageSettingsAdaptor blobStorageSettings)
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
            var result = await Mediator.Send(new UploadTrackSupportingDetailsCommand
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
    }
}
