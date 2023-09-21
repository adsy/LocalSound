using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using localsound.backend.api.Queries.Track;
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

        [HttpGet("member/{memberId}/upload-token")]
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
    }
}
