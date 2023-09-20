using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IUploadTrackRepository
    {
        Task<ServiceResponse<ArtistTrackChunk>> UploadTrackChunkAsync(string fileLocation, Guid partialTrackId, Guid userId, int chunkId);
        Task<ServiceResponse<ArtistTrackUpload>> AddArtistTrackToDbAsync(Guid userId, Guid trackId, Guid genreId, string trackName, string trackDescription, string trackFileExt, string trackLocation, Guid trackImageId, string trackImageFileExt, string trackImageLocation);
        Task<ServiceResponse<List<ArtistTrackChunkDto>>> GetPartialTrackChunksAsync(Guid partialTrackId);
        Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(Guid trackId);
        Task<ServiceResponse> DeletePartialTrackChunksAysnc(Guid partialTrackId);
        Task<ServiceResponse> SetTrackReadyAsync(Guid trackId);
    }
}
