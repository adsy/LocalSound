using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<CustomerType>> AddNonArtistToDbAsync(NonArtist nonArtist);
        Task<ServiceResponse<CustomerType>> AddArtistToDbAsync(Artist artist);
        Task<ServiceResponse<Artist>> GetArtistFromDbAsync(Guid id);
        Task<ServiceResponse<Artist>> GetArtistFromDbAsync(string profileUrl);
        Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(Guid id);
        Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(string profileUrl);
        Task<ServiceResponse<AppUser>> GetAppUserFromDbAsync(Guid id, string memberId);
        Task<ServiceResponse<AppUser>> GetAppUserFromDbAsync(string memberId);
        Task<ServiceResponse<AccountImage>> GetAccountImageFromDbAsync(Guid id, AccountImageTypeEnum imageType);
        Task<ServiceResponse<List<ArtistFollower>>> GetArtistFollowersFromDbAsync(string memberId, int page, CancellationToken cancellationToken);
        Task<ServiceResponse<List<ArtistFollower>>> GetArtistFollowingFromDbAsync(string memberId, int page, CancellationToken cancellationToken);
    }
}
