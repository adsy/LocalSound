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
        Task<ServiceResponse> DeleteArtistPackagePhotoAsync(ArtistPackagePhoto photo);
        Task<ServiceResponse> UpdateArtistPackageEquipmentAsync(Guid PackageId, List<ArtistPackageEquipment> equipment);
        Task<ServiceResponse> UpdateArtistPackageAsync(Guid packageId, string name, string description, string price, List<ArtistPackagePhoto> newPhotos, List<ArtistPackagePhoto> deletedPhotos);
    }
}
