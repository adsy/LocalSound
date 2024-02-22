using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITrackRepository
    {
        Task<ServiceResponse<int>> AddArtistTrackUploadAsync(ArtistTrack track);
        Task<ServiceResponse> MarkTrackForDeletion(string memberId, int trackId);
        Task<ServiceResponse<ArtistTrack>> GetArtistTrackAsync(string memberId, int trackId);
        Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetArtistTracksAsync(string memberId, int? lastTrackId);
        Task<ServiceResponse> LikeArtistTrackAsync(string memberId, string artistMemberId, int trackId);
        Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, int songLikeId);
        Task<ServiceResponse> UpdateArtistTrackUploadAsync(ArtistTrack track, string trackName, string trackDescription, List<GenreDto> genres, ArtistTrackImage? newTrackImage);
        Task<ServiceResponse<List<int>>> GetLikedSongsIdsAsync(string memberId);
        Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetLikedSongsAsync(string memberId, int? lastTrackId);
    }
}
