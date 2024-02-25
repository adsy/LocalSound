using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface ITrackService
    {
        Task<ServiceResponse> DeleteArtistTrackAsync(Guid userId, string memberId, int trackId);
        Task<ServiceResponse<TrackUploadSASDto>> GenerateTrackUploadSasDtoAsync(Guid userId, string memberId);
        Task<ServiceResponse<ArtistTrackUploadDto>> GetArtistTrackAsync(string memberId, int trackId);
        Task<ServiceResponse<TrackListResponseDto>> GetTracksByPlaylistTypeAsync(Guid? userId, string memberId, int? lastTrackId, PlaylistTypeEnum playlistType);
        Task<ServiceResponse> LikeArtistTrackAsync(int trackId, string artistMemberId, Guid userId, string memberId);
        Task<ServiceResponse> UnlikeArtistTrackAsync(Guid userId, string memberId, int songLikeId);
        Task<ServiceResponse> UpdateTrackSupportingDetailsAsync(Guid userId, string memberId, int trackId, TrackUpdateDto trackData);
        Task<ServiceResponse<int>> UploadTrackSupportingDetailsAsync(Guid userId, string memberId, TrackUploadDto trackData);
    }
}
