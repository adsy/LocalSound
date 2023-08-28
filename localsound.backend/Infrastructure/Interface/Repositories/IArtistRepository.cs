using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IArtistRepository
    {
        Task<ServiceResponse> UpdateArtistDetailsAsync(Guid userId, UpdateArtistDto updateArtistDto);
    }
}
