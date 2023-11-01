using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITrackRepository
    {
        Task<ServiceResponse> AddArtistTrackUploadAsync(ArtistTrackUpload track);
        Task<ServiceResponse<List<ArtistTrackUpload>>> GetArtistTracksAsync(string memberId, int page);
    }
}
