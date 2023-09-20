using localsound.backend.api.Commands.Track;
using localsound.backend.Domain.Model.Dto.Submission;
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
        public async Task<ActionResult> UploadComplete(string memberId, Guid partialTrackId)
        {
            var result = await Mediator.Send(new CompleteTrackUploadCommand
            {
                MemberId = memberId,
                PartialTrackId = partialTrackId,
                AppUserId = CurrentUser.AppUserId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode);
            }

            return Ok();
        }
    }
}
