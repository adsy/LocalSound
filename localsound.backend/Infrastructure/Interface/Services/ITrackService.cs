using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface ITrackService
    {
        Task<ServiceResponse<TrackUploadSASDto>> GenerateTrackUploadSASDto(Guid userId, string memberId);
        Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetArtistTracks(string memberId);
        Task<ServiceResponse> UploadTrackSupportingDetails(Guid userId, string memberId, Guid trackId, TrackUploadDto trackData);
    }
}
