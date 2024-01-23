using localsound.CoreUpdates.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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

        public async Task<bool> DeleteAccountImageAsync(Guid userId, int accountImageId)
        {
            try
            {
                var accountImage = await _dbContext.AccountImage
                    .Include(x => x.FileContent)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId && x.AccountImageId == accountImageId);

                if (accountImage == null || accountImage.FileContent == null)
                    return false;

                _dbContext.FileContent.Remove(accountImage.FileContent);

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
                var packagePhotos = await _dbContext.ArtistPackagePhoto
                    .Include(x => x.FileContent)
                    .Where(x => x.ArtistPackageId == packageId && x.ToBeDeleted)
                    .ToListAsync();

                foreach(var packagePhoto in packagePhotos)
                {
                    _dbContext.FileContent.Remove(packagePhoto.FileContent);
                }

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
    }
}
