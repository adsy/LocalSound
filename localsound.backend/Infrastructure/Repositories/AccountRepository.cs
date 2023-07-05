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

        public async Task<ServiceResponse<Artist>> AddArtistToDbAsync(Artist artist)
        {
            try
            {
                var fnResult = new ServiceResponse<Artist>(HttpStatusCode.BadRequest);

                if (artist == null)
                {
                    return fnResult;
                }

                var result = await _dbContext.Artist.FirstOrDefaultAsync(o => o.AppUserId == artist.User.Id);

                if (result != null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(AddArtistToDbAsync)} - Server error adding artist into Artist database - " +
                        $"Id:{artist.User.Id} already exists in customer database";
                    _logger.LogError(message);
                    return fnResult;
                }

                var artistResult = await _dbContext.Artist.AddAsync(artist);

                await _dbContext.SaveChangesAsync();

                fnResult.StatusCode = HttpStatusCode.OK;
                fnResult.ReturnData = artistResult.Entity;

                return fnResult;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(AddArtistToDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Artist>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<NonArtist>> AddNonArtistToDbAsync(NonArtist nonArtist)
        {
            try
            {
                var fnResult = new ServiceResponse<NonArtist>(HttpStatusCode.BadRequest);

                if (nonArtist == null)
                {
                    return fnResult;
                }

                var result = await _dbContext.NonArtist.FirstOrDefaultAsync(o => o.AppUserId == nonArtist.User.Id);

                if (result != null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(AddNonArtistToDbAsync)} - Server error adding NonArtist into NonArtist database - " +
                        $"Id:{nonArtist.User.Id} already exists in customer database";
                    _logger.LogError(message);
                    return fnResult;
                }

                var artistResult = await _dbContext.NonArtist.AddAsync(nonArtist);

                await _dbContext.SaveChangesAsync();

                fnResult.StatusCode = HttpStatusCode.OK;
                fnResult.ReturnData = artistResult.Entity;

                return fnResult;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(AddNonArtistToDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<NonArtist>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Artist>> GetArtistFromDbAsync(Guid id)
        {
            try
            {
                var artist = await _dbContext.Artist.Include(x => x.User).FirstOrDefaultAsync(x => x.AppUserId == id);

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

        public async Task<ServiceResponse<Artist>> GetArtistFromDbAsync(string profileUrl)
        {
            try
            {
                var artist = await _dbContext.Artist.Include(x => x.User).FirstOrDefaultAsync(x => x.ProfileUrl == profileUrl);

                if (artist == null)
                {
                    return new ServiceResponse<Artist>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<Artist>(HttpStatusCode.OK)
                {
                    ReturnData = artist
                };
            }
            catch (Exception e)
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
                var nonArtist = await _dbContext.NonArtist.Include(x => x.User).FirstOrDefaultAsync(x => x.AppUserId == id);

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

        public async Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(string profileUrl)
        {
            try
            {
                var nonArtist = await _dbContext.NonArtist.Include(x => x.User).FirstOrDefaultAsync(x => x.ProfileUrl == profileUrl);

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
