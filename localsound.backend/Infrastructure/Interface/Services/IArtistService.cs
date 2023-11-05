using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IArtistService
    {
        Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, string memberId, UpdateArtistPersonalDetailsDto updateArtistDto);
        Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, string memberId, UpdateArtistProfileDetailsDto updateArtistDto);
        Task<ServiceResponse> FollowArtist(Guid userId, string followerId, string artistId);
    }
}
