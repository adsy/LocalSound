using localsound.backend.api.Commands.Track;
using localsound.backend.api.Queries.Track;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
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
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpPost]
        [Route("member/{memberId}/track/{trackId}")]
        public async Task<ActionResult> UploadTrackSupportingDetails([FromForm] TrackUploadDto data, string memberId, Guid trackId)
        {
            var result = await Mediator.Send(new AddTrackSupportingDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = trackId,
                TrackData = data
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpPut]
        [Route("member/{memberId}/track/{trackId}")]
        public async Task<ActionResult> UpdateTrackSupportingDetails([FromForm] TrackUpdateDto data, string memberId, Guid trackId)
        {
            var result = await Mediator.Send(new UpdateTrackSupportingDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = trackId,
                TrackData = data
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpGet]
        [Route("member/{memberId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetArtistTrackUploads(string memberId, [FromQuery]int page)
        {
            var result = await Mediator.Send(new GetArtistTracksQuery
            {
                MemberId = memberId,
                Page = page
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpGet]
        [Route("member/{memberId}/track/{trackId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetArtistTrack(string memberId, Guid trackId)
        {
            var result = await Mediator.Send(new GetArtistTrackQuery
            {
                MemberId = memberId,
                TrackId = trackId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpDelete]
        [Route("member/{memberId}/track/{trackId}")]
        public async Task<ActionResult> DeleteArtistTrack(string memberId, Guid trackId)
        {
            var result = await Mediator.Send(new DeleteArtistTrackCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = trackId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }
    }
}
