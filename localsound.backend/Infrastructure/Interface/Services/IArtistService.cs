using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IArtistService
    {
        Task<ServiceResponse> UpdateArtistDetails(Guid userId, string memberId, UpdateArtistDto updateArtistDto);
    }
}
