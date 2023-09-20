using localsound.backend.api.Authentication;
using localsound.backend.api.Commands.Track;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/upload-track")]
    [ApiController]
    public class UploadTrackController : BaseApiController
    {
        [Route("member/{memberId}/partial-track-id/{partialTrackId}/chunk/{chunkId}")]
        [HttpPost]
        public async Task<ActionResult> UploadFile([FromForm] FileUploadDto formData, string memberId, Guid partialTrackId, int chunkId)
        {
            var result = await Mediator.Send(new UploadTrackChunkCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId, 
                PartialTrackId = partialTrackId,
                File = formData.FormFile,
                ChunkId = chunkId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok();
        }

        [Route("member/{memberId}/partial-track-id/{partialTrackId}/upload-complete")]
        [HttpPost]
        public async Task<ActionResult> UploadComplete([FromForm] TrackUploadDto formData, string memberId, Guid partialTrackId)
        {
            var result = await Mediator.Send(new CompleteTrackUploadCommand
            {
                MemberId = memberId,
                PartialTrackId = partialTrackId,
                AppUserId = CurrentUser.AppUserId,
                FormData = formData
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok();
        }

        [Route("partial-track-id/{partialTrackId}/track/{trackId}/merge-chunks")]
        [HttpPost]
        [Authorize(ApiKeyAuthenticationScheme.ApiKeyScheme)]
        public async Task<ActionResult> TriggerChunkMerge(Guid partialTrackId, Guid trackId)
        {
            var result = await Mediator.Send(new TriggerTrackMergeCommand
            {
                PartialTrackId = partialTrackId,
                TrackId = trackId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok();
        }
    }
}
