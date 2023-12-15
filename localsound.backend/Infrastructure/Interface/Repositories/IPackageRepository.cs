using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IPackageRepository
    {
        Task<ServiceResponse> CreateArtistPackageAsync(ArtistPackage artistPackage);
        Task<ServiceResponse> DeleteArtistPackageAsync(ArtistPackage artistPackage);
        Task<ServiceResponse<ArtistPackage>> GetArtistPackageAsync(Guid appUserId, Guid packageId);
        Task<ServiceResponse<List<ArtistPackage>>> GetArtistPackagesAsync(string memberId);
    }
}
