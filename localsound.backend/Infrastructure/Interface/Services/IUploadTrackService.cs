using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IUploadTrackService
    {
        Task<ServiceResponse<TrackUploadSASDto>> GenerateTrackUploadSASDto(Guid userId, string memberId);
    }
}
