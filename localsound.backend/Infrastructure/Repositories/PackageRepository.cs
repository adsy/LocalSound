using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Services;
using localsound.backend.Persistence.DbContext;
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
    }
}
