using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITrackRepository
    {
        Task<ServiceResponse> AddArtistTrackUploadAsync(ArtistTrackUpload track);
        Task<ServiceResponse> DeleteTrackAsync(ArtistTrackUpload track);
        Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(string memberId, Guid trackId);
        Task<ServiceResponse<List<ArtistTrackUpload>>> GetArtistTracksAsync(string memberId, DateTime? lastUploadDate);
        Task<ServiceResponse> LikeArtistTrackAsync(string memberId, string artistMemberId, Guid trackId);
        Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, string artistMemberId, Guid trackId);
        Task<ServiceResponse> UpdateArtistTrackUploadAsync(Account account, Guid trackId, string trackName, string trackDescription, List<GenreDto> genres, string? trackImageExt, FileContent? newTrackImage, string newTrackImageUrl);
        Task<ServiceResponse<List<Guid>>> GetLikedSongsIdsAsync(string memberId);
        Task<ServiceResponse<List<ArtistTrackUpload>>> GetLikedSongsAsync(string memberId, DateTime? lastUploadDate);
    }
}
