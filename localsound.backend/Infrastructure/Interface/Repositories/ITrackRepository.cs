using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITrackRepository
    {
        Task<ServiceResponse<int>> AddArtistTrackUploadAsync(ArtistTrackUpload track);
        Task<ServiceResponse> DeleteTrackAsync(ArtistTrackUpload track);
        Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(string memberId, int trackId);
        Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetArtistTracksAsync(string memberId, int? lastTrackId);
        Task<ServiceResponse> LikeArtistTrackAsync(string memberId, string artistMemberId, int trackId);
        Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, int songLikeId);
        Task<ServiceResponse> UpdateArtistTrackUploadAsync(Account account, int trackId, string trackName, string trackDescription, List<GenreDto> genres, string? trackImageExt, ArtistTrackImageFileContent? newTrackImage, string newTrackImageUrl);
        Task<ServiceResponse<List<int>>> GetLikedSongsIdsAsync(string memberId);
        Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetLikedSongsAsync(string memberId, int? lastTrackId);
    }
}
