using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IPackageService
    {
        Task<ServiceResponse> CreateArtistPackage(Guid appUserId, string memberId, CreatePackageDto packageDto);
        Task<ServiceResponse> DeleteArtistPackage(Guid appUserId, string memberId, Guid packageId);
        Task<ServiceResponse<List<ArtistPackageDto>>> GetArtistPackages(string memberId);
    }
}
