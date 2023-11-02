using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITrackRepository
    {
        Task<ServiceResponse> AddArtistTrackUploadAsync(ArtistTrackUpload track);
        Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(string memberId, Guid trackId);
        Task<ServiceResponse<List<ArtistTrackUpload>>> GetArtistTracksAsync(string memberId, int page);
        Task<ServiceResponse> UpdateArtistTrackUploadAsync(AppUser appUser, Guid trackId, string trackName, string trackDescription, List<GenreDto> genres, string? trackImageExt, FileContent? newTrackImage);
    }
}
