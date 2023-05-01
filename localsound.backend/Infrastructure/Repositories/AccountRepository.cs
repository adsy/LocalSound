using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Services;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(LocalSoundDbContext dbContext, ILogger<AccountRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<Artist>> GetArtistFromDbAsync(Guid id)
        {
            try
            {
                var artist = await _dbContext.Artist.FirstOrDefaultAsync(x => x.AppUserId == id);

                if (artist == null)
                {
                    return new ServiceResponse<Artist>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<Artist>(HttpStatusCode.OK)
                {
                    ReturnData = artist
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Artist>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(Guid id)
        {
            try
            {
                var nonArtist = await _dbContext.NonArtist.FirstOrDefaultAsync(x => x.AppUserId == id);

                if (nonArtist == null)
                {
                    return new ServiceResponse<NonArtist>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<NonArtist>(HttpStatusCode.OK)
                {
                    ReturnData = nonArtist
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetNonArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<NonArtist>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
