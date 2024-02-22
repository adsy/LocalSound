using localsound.CoreUpdates.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Repository
{
    public interface IDbOperationRepository
    {
        Task<List<string>> GetPackagePhotoLocations(Guid userId, Guid packageId);
        Task<string> GetAccountImageLocation(Guid userId, int accountImageId);
        Task<List<string>> GetArtistTrackLocations(int artistTrackId, string artistMemberId);
        Task<bool> DeleteAccountImageAsync(Guid userId, int accountImageId);
        Task<bool> DeletePackagePhotosAsync(Guid userId, Guid packageId);
        Task<bool> DeleteArtistTrackAsync(int artistTrackId, string artistMemberId);
        Task<ArtistTrackImageDto> GetArtistTrackImageLocation(int artistTrackId, string artistMemberId);
        Task<bool> DeleteArtistTrackImageAsync(int artistTrackImageId);
    }
}
