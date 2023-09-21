using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Services;
using localsound.backend.Persistence.DbContext;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class UploadTrackRepository : IUploadTrackRepository
    {
        public readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<UploadTrackRepository> _logger;

        public UploadTrackRepository(LocalSoundDbContext dbContext, ILogger<UploadTrackRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> AddArtistTrackUploadAsync(ArtistTrackUpload track)
        {
            try
            {
                await _dbContext.ArtistTrackUpload.AddAsync(track);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(AddArtistTrackUploadAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<IAppUserDto>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
