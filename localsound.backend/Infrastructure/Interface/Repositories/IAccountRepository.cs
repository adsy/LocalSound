using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<CustomerType>> AddNonArtistToDbAsync(Account nonArtist);
        Task<ServiceResponse<CustomerType>> AddArtistToDbAsync(Account artist);
        Task<ServiceResponse<Account>> GetArtistFromDbAsync(Guid id);
        Task<ServiceResponse<Account>> GetArtistFromDbAsync(string profileUrl);
        Task<ServiceResponse<Account>> GetNonArtistFromDbAsync(Guid id);
        Task<ServiceResponse<Account>> GetNonArtistFromDbAsync(string profileUrl);
        Task<ServiceResponse<Account>> GetAccountFromDbAsync(Guid id);
        Task<ServiceResponse<Account>> GetAccountFromDbAsync(string memberId);
        Task<ServiceResponse<Account>> GetAccountFromDbAsync(Guid id, string memberId);
        Task<ServiceResponse<AccountImage>> GetAccountImageFromDbAsync(Guid id, AccountImageTypeEnum imageType);
        Task<ServiceResponse<List<ArtistFollower>>> GetArtistFollowersFromDbAsync(string memberId, int page, CancellationToken cancellationToken);
        Task<ServiceResponse<List<ArtistFollower>>> GetProfileFollowingFromDbAsync(string memberId, int page, CancellationToken cancellationToken);
        Task<ServiceResponse> SaveOnboardingData(Guid userId, SaveOnboardingDataDto onboardingData);
        Task<ServiceResponse> UpdateArtistFollowerAsync(Account follower, string artistId, bool startFollowing);
        Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, UpdateArtistPersonalDetailsDto updateArtistDto);
        Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, UpdateArtistProfileDetailsDto updateArtistDto);
    }
}
