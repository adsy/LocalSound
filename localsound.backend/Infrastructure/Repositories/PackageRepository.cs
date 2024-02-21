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

        public async Task<ServiceResponse> MarkPackageAsUnavailable(ArtistPackage package)
        {
            try
            {
                foreach(var photo in package.PackagePhotos)
                {
                    photo.ToBeDeleted = true;
                }

                package.IsAvailable = false;

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }   
            catch(Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(MarkPackageAsUnavailable)} - {e.Message}";
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
                    .Include(x => x.Equipment)
                    .Include(x => x.PackagePhotos)
                    .ThenInclude(x => x.ArtistPackageImageFileContent)
                    .Select(x => new ArtistPackage
                    {
                        ArtistPackageId = x.ArtistPackageId,
                        AppUserId = x.AppUserId,
                        PackageName = x.PackageName,
                        PackageDescription = x.PackageDescription,
                        PackagePrice = x.PackagePrice,
                        IsAvailable = x.IsAvailable,
                        Equipment = x.Equipment,
                        PackagePhotos = x.PackagePhotos.Where(x => !x.ToBeDeleted).Select(photo => new ArtistPackageImage
                        {
                            ArtistPackageImageId = photo.ArtistPackageImageId,
                            ArtistPackageId = photo.ArtistPackageId,
                            PhotoUrl = photo.PhotoUrl,
                            ToBeDeleted = photo.ToBeDeleted,
                            ArtistPackageImageFileContent = photo.ArtistPackageImageFileContent
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.ArtistPackageId == packageId && x.AppUserId == appUserId);

                if (package is null)
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
                var artist = await _dbContext.Account.FirstOrDefaultAsync(x => x.MemberId == memberId);

                if (artist is null)
                {
                    return new ServiceResponse<List<ArtistPackage>>(HttpStatusCode.NotFound, "An error occured getting artist packages, please try again...");
                }

                var packages = await _dbContext.ArtistPackage
                    .Include(x => x.Equipment)
                    .Include(x => x.PackagePhotos)
                    .Where(x => x.AppUserId == artist.AppUserId && x.IsAvailable)
                    .Select(x => new ArtistPackage
                    {
                        ArtistPackageId = x.ArtistPackageId,
                        AppUserId = x.AppUserId, 
                        PackageName = x.PackageName,
                        PackageDescription = x.PackageDescription,
                        PackagePrice = x.PackagePrice,
                        IsAvailable = x.IsAvailable,
                        Equipment = x.Equipment,
                        PackagePhotos = x.PackagePhotos.Where(x => !x.ToBeDeleted).Select(photo => new ArtistPackageImage
                        {
                            ArtistPackageImageId = photo.ArtistPackageImageId,
                            ArtistPackageId = photo.ArtistPackageId,
                            PhotoUrl = photo.PhotoUrl,
                            ToBeDeleted = photo.ToBeDeleted,
                            ArtistPackageImageFileContent = photo.ArtistPackageImageFileContent
                        }).ToList()
                    })
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

        public async Task<ServiceResponse> UpdateArtistPackageEquipmentAsync(Guid AppUserId, Guid PackageId, List<ArtistPackageEquipment> equipment)
        {
            try
            {
                var package = await _dbContext.ArtistPackage
                    .Include(x => x.Equipment)
                    .FirstOrDefaultAsync(x => x.ArtistPackageId == PackageId && x.AppUserId == AppUserId);

                if (package is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured updating your package, please try again..."
                    };
                }

                package.UpdateEquipment(equipment);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(UpdateArtistPackageEquipmentAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured updating your package, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UpdateArtistPackageAsync(Guid packageId, string name, string description, string price, List<ArtistPackageImage> newPhotos)
        {
            try
            {
                var package = await _dbContext.ArtistPackage
                    .Include(x => x.PackagePhotos)
                    .ThenInclude(x => x.ArtistPackageImageFileContent)
                    .FirstOrDefaultAsync(x => x.ArtistPackageId == packageId);

                if (package is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured updating your package, please try again..."
                    };
                }

                // Add new FileContent entries for FK reference
                if (newPhotos.Any())
                {
                    foreach(var photo in newPhotos)
                    {
                        await _dbContext.ArtistPackageImage.AddAsync(photo);
                    }
                }

                package.UpdateDetails(name, description, price)
                    .UpdatePhotos(newPhotos);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(UpdateArtistPackageAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured updating your package, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> MarkPhotosForDeletion(Guid packageId, List<int> deletedIds)
        {
            try
            {
                var packagePhotos = await _dbContext.ArtistPackageImage
                    .Where(x => x.ArtistPackageId == packageId && deletedIds.Contains(x.ArtistPackageImageId)).ToListAsync();

                foreach(var photo in packagePhotos)
                {
                    photo.ToBeDeleted = true;
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(PackageRepository)} - {nameof(UpdateArtistPackageAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured updating your package, please try again..."
                };
            }
        }
    }
}
