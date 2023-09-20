using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IUploadTrackService
    {
        Task<ServiceResponse> UploadFileChunk(Guid userId, string memberId, Guid partialTrackId, IFormFile file, int chunkId);
        Task<ServiceResponse> CompleteTrackUpload(Guid userId, string memberId, Guid partialTrackId, TrackUploadDto formData);
        Task<ServiceResponse> MergeTrackChunks(Guid partialTrackId, Guid trackId);
    }
}
