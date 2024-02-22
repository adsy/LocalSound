using localsound.CoreUpdates.Dto;
using localsound.CoreUpdates.Persistence;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Repository
{
    public class DbOperationRepository : IDbOperationRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly Logger<DbOperationRepository> _logger;

        public DbOperationRepository(LocalSoundDbContext dbContext, Logger<DbOperationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<string> GetAccountImageLocation(Guid userId, int accountImageId)
        {
            try
            {
                var accountImage = await _dbContext.AccountImage
                    .Include(x => x.AccountImageFileContent)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId && x.AccountImageId == accountImageId);

                if (accountImage is null || accountImage.AccountImageFileContent is null)
                    return null;

                return accountImage.AccountImageFileContent.FileLocation;
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(GetAccountImageLocation)} - {e.Message}";
                _logger.LogError(e, message);
                return null;
            }
        }

        public async Task<List<string>> GetPackagePhotoLocations(Guid userId, Guid packageId)
        {
            try
            {
                var package = await _dbContext.ArtistPackage
                    .Include(x => x.PackagePhotos)
                    .ThenInclude(x => x.ArtistPackageImageFileContent)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId && x.ArtistPackageId == packageId);

                if (package is null || !package.PackagePhotos.Any())
                    return null;

                return package.PackagePhotos.Select(x => x.ArtistPackageImageFileContent.FileLocation).ToList();
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(GetPackagePhotoLocations)} - {e.Message}";
                _logger.LogError(e, message);
                return null;
            }
        }

        public async Task<List<string>> GetArtistTrackLocations(int artistTrackId, string artistMemberId)
        {
            try
            {
                var artistTrack = await _dbContext.ArtistTrack
                    .Include(x => x.TrackImage)
                    .ThenInclude(x => x.ArtistTrackImageFileContent)
                    .Include(x => x.TrackData)
                    .FirstOrDefaultAsync(x => x.ArtistTrackId == artistTrackId && x.ArtistMemberId == artistMemberId);

                if (artistTrack is null)
                    return null;

                var fileLocations = artistTrack.TrackImage.Select(x => x.ArtistTrackImageFileContent.FileLocation).ToList();

                fileLocations.Add(artistTrack.TrackData.FileLocation);

                return fileLocations;
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(GetArtistTrackLocations)} - {e.Message}";
                _logger.LogError(e, message);
                return null;
            }
        }

        public async Task<ArtistTrackImageDto> GetArtistTrackImageLocation(int artistTrackId, string artistMemberId)
        {
            try
            {
                var artistTrack = await _dbContext.ArtistTrack
                    .Include(x => x.TrackImage)
                    .ThenInclude(x => x.ArtistTrackImageFileContent)
                    .Include(x => x.TrackData)
                    .FirstOrDefaultAsync(x => x.ArtistTrackId == artistTrackId && x.ArtistMemberId == artistMemberId);

                if (artistTrack is null)
                    return null;

                var trackImage = artistTrack.TrackImage.FirstOrDefault(x => x.ToBeDeleted);

                return new ArtistTrackImageDto
                {
                    ArtistTrackImageId = trackImage.ArtistTrackImageId,
                    ArtistTrackImageFileLocation = trackImage.ArtistTrackImageFileContent.FileLocation
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(GetArtistTrackImageLocation)} - {e.Message}";
                _logger.LogError(e, message);
                return null;
            }
        }

        public async Task<bool> DeleteAccountImageAsync(Guid userId, int accountImageId)
        {
            try
            {
                var accountImage = await _dbContext.AccountImage
                    .Include(x => x.AccountImageFileContent)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId && x.AccountImageId == accountImageId);

                if (accountImage is null || accountImage.AccountImageFileContent is null)
                    return false;

                _dbContext.AccountImage.Remove(accountImage);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(DeleteAccountImageAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return false;
            }
        }

        public async Task<bool> DeletePackagePhotosAsync(Guid userId, Guid packageId)
        {
            try
            {
                var packagePhotos = await _dbContext.ArtistPackageImage
                    .Include(x => x.ArtistPackage)
                    .Include(x => x.ArtistPackageImageFileContent)
                    .Where(x => x.ArtistPackage.AppUserId == userId && x.ArtistPackageId == packageId && x.ToBeDeleted)
                    .ToListAsync();

                _dbContext.ArtistPackageImage.RemoveRange(packagePhotos);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(DeletePackagePhotosAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return false;
            }
        }

        public async Task<bool> DeleteArtistTrackAsync(int artistTrackId, string artistMemberId)
        {
            try
            {
                var artistTrack = await _dbContext.ArtistTrack
                    .Include(x => x.TrackImage)
                    .ThenInclude(x => x.ArtistTrackImageFileContent)
                    .Include(x => x.TrackData)
                    .Include(x => x.Genres)
                    .Include(x => x.SongLikes)
                    .FirstOrDefaultAsync(x => x.ArtistTrackId == artistTrackId && x.ArtistMemberId == artistMemberId);

                _dbContext.ArtistTrack.Remove(artistTrack);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(DeletePackagePhotosAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return false;
            }
        }

        public async Task<bool> DeleteArtistTrackImageAsync(int artistTrackImageId)
        {
            try
            {
                var aristTrackImage = await _dbContext.ArtistTrackImage
                    .Include(x => x.ArtistTrackImageFileContent)
                    .FirstOrDefaultAsync(x => x.ArtistTrackImageId == artistTrackImageId);

                _dbContext.ArtistTrackImage.Remove(aristTrackImage);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception e)
            {
                var message = $"{nameof(DbOperationRepository)} - {nameof(DeleteArtistTrackImageAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return false;
            }
        }
    }
}
