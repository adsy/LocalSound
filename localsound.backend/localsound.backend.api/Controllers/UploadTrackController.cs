using localsound.backend.api.Commands.File;
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
            var result = await Mediator.Send(new UploadFileChunkCommand
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId, 
                PartialTrackId = partialTrackId,
                File = formData.FormFile,
                ChunkId = chunkId
            });

            if (!result.IsSuccessStatusCode)
            {
                // if fails, need to push a service bus event to delete from artist storage to remove broken chunks

                return StatusCode((int)result.StatusCode);
            }

            return Ok();
        }
    }
}
