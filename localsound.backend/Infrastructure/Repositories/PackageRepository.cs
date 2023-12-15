using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<PackageRepository> _logger;

        public PackageRepository(LocalSoundDbContext dbContext, ILogger<PackageRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> CreateArtistPackageAsync(ArtistPackage artistPackage)
        {
            try
            {
                await _dbContext.ArtistPackage.AddAsync(artistPackage);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(CreateArtistPackageAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured creating your package, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> DeleteArtistPackageAsync(ArtistPackage package)
        {
            try
            {
                // Delete photo content if it exists
                if (package.PackagePhotos.Any())
                {
                    foreach(var photo in package.PackagePhotos)
                    {
                        _dbContext.FileContent.Remove(photo.FileContent);
                    }
                }

                _dbContext.ArtistPackage.Remove(package);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }   
            catch(Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(DeleteArtistPackageAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured deleting your package, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<ArtistPackage>> GetArtistPackageAsync(Guid appUserId, Guid packageId)
        {
            try
            {
                var package = await _dbContext.ArtistPackage
                    .Include(x => x.PackagePhotos)
                    .ThenInclude(x => x.FileContent)
                    .FirstOrDefaultAsync(x => x.ArtistPackageId == packageId && x.AppUserId == appUserId);

                if (package == null)
                {
                    return new ServiceResponse<ArtistPackage>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<ArtistPackage>(HttpStatusCode.OK)
                {
                    ReturnData = package
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(GetArtistPackagesAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistPackage>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting artist packages, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<List<ArtistPackage>>> GetArtistPackagesAsync(string memberId)
        {
            try
            {
                var artist = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.MemberId == memberId);

                if (artist == null)
                {
                    return new ServiceResponse<List<ArtistPackage>>(HttpStatusCode.NotFound, "An error occured getting artist packages, please try again...");
                }

                var packages = await _dbContext.ArtistPackage
                    .Include(x => x.Equipment)
                    .Include(x => x.PackagePhotos)
                    .Where(x => x.AppUserId == artist.Id)
                    .ToListAsync();

                packages = packages.OrderBy(x => double.Parse(x.PackagePrice)).ToList();

                return new ServiceResponse<List<ArtistPackage>>(HttpStatusCode.OK)
                {
                    ReturnData = packages
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(GetArtistPackagesAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistPackage>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting artist packages, please try again..."
                };
            }
        }
    }
}
