using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IUploadTrackRepository
    {
        Task<ServiceResponse<ArtistTrackChunk>> UploadTrackChunkAsync(string fileLocation, Guid partialTrackId, Guid userId, int chunkId);
    }
}
