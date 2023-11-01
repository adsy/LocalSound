using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        public readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<TrackRepository> _logger;

        public TrackRepository(LocalSoundDbContext dbContext, ILogger<TrackRepository> logger)
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
                var message = $"{nameof(TrackRepository)} - {nameof(AddArtistTrackUploadAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackUpload>>> GetArtistTracksAsync(string memberId, int page)
        {
            try
            {
                var artist = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.MemberId == memberId && x.CustomerType == CustomerTypeEnum.Artist);

                if (artist == null)
                {
                    return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.NotFound);
                }

                var tracks = await _dbContext.ArtistTrackUpload
                    .Include(x => x.TrackData)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Where(x => x.AppUserId == artist.Id)
                    .OrderByDescending(x => x.UploadDate)
                    .Skip(10 * page)
                    .Take(10)
                    .ToListAsync();

                return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.OK)
                {
                    ReturnData = tracks
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetArtistTracksAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
