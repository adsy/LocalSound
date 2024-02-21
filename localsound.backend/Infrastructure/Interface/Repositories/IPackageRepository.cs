using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IPackageRepository
    {
        Task<ServiceResponse> CreateArtistPackageAsync(ArtistPackage artistPackage);
        Task<ServiceResponse> MarkPackageAsUnavailable(ArtistPackage artistPackage);
        Task<ServiceResponse<ArtistPackage>> GetArtistPackageAsync(Guid appUserId, Guid packageId);
        Task<ServiceResponse<List<ArtistPackage>>> GetArtistPackagesAsync(string memberId);
        Task<ServiceResponse> UpdateArtistPackageEquipmentAsync(Guid appUserId, Guid PackageId, List<ArtistPackageEquipment> equipment);
        Task<ServiceResponse> UpdateArtistPackageAsync(Guid packageId, string name, string description, string price, List<ArtistPackageImage> newPhotos);
        Task<ServiceResponse> MarkPhotosForDeletion(Guid packageId, List<int> deletedIds);
    }
}
