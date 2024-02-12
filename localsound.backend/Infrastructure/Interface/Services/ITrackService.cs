using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface ITrackService
    {
        Task<ServiceResponse> DeleteArtistTrackAsync(Guid userId, string memberId, Guid trackId);
        Task<ServiceResponse<TrackUploadSASDto>> GenerateTrackUploadSASDtoAsync(Guid userId, string memberId);
        Task<ServiceResponse<ArtistTrackUploadDto>> GetArtistTrackAsync(string memberId, Guid trackId);
        Task<ServiceResponse<TrackListResponseDto>> GetArtistTracksAsync(Guid? userId, string memberId, int page);
        Task<ServiceResponse> LikeArtistTrackAsync(Guid trackId, string artistMemberId, Guid userId, string memberId);
        Task<ServiceResponse> UnikeArtistTrackAsync(Guid trackId, string artistMemberId, Guid userId, string memberId);
        Task<ServiceResponse> UpdateTrackSupportingDetailsAsync(Guid userId, string memberId, Guid trackId, TrackUpdateDto trackData);
        Task<ServiceResponse> UploadTrackSupportingDetailsAsync(Guid userId, string memberId, Guid trackId, TrackUploadDto trackData);
    }
}
