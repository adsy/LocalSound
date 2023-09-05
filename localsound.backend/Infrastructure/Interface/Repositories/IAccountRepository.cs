using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IAccountRepository
    {
        Task<ServiceResponse<NonArtist>> AddNonArtistToDbAsync(NonArtist nonArtist);
        Task<ServiceResponse<Artist>> AddArtistToDbAsync(Artist artist);
        Task<ServiceResponse<Artist>> GetArtistFromDbAsync(Guid id);
        Task<ServiceResponse<Artist>> GetArtistFromDbAsync(string profileUrl);
        Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(Guid id);
        Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(string profileUrl);
        Task<ServiceResponse<AppUser>> GetAppUserFromDbAsync(Guid id, string memberId);
        Task<ServiceResponse<AccountImage>> GetAccountImageFromDbAsync(Guid id, AccountImageTypeEnum imageType);
    }
}
