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
    public class AccountRepository : IAccountRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(LocalSoundDbContext dbContext, ILogger<AccountRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<CustomerType>> AddArtistToDbAsync(Artist artist)
        {
            try
            {
                var fnResult = new ServiceResponse<CustomerType>(HttpStatusCode.BadRequest);

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

                var existingProfileWithUrl = await _dbContext.Artist.FirstOrDefaultAsync(o => o.ProfileUrl == artist.ProfileUrl);

                if (existingProfileWithUrl != null)
                {
                    fnResult.ServiceResponseMessage = "That profile URL is already in use, please try another one.";
                }
                else
                {
                    var artistResult = await _dbContext.Artist.AddAsync(artist);

                    await _dbContext.SaveChangesAsync();

                    fnResult.StatusCode = HttpStatusCode.OK;
                    fnResult.ReturnData = artistResult.Entity;
                }

                return fnResult;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(AddArtistToDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<CustomerType>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<CustomerType>> AddNonArtistToDbAsync(NonArtist nonArtist)
        {
            try
            {
                var fnResult = new ServiceResponse<CustomerType>(HttpStatusCode.BadRequest);

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

                var existingProfileWithUrl = await _dbContext.NonArtist.FirstOrDefaultAsync(o => o.ProfileUrl == nonArtist.ProfileUrl);

                if (existingProfileWithUrl != null)
                {
                    fnResult.ServiceResponseMessage = "That profile URL is already in use, please try another one.";
                }
                else
                {
                    var nonartistResult = await _dbContext.NonArtist.AddAsync(nonArtist);

                    await _dbContext.SaveChangesAsync();

                    fnResult.StatusCode = HttpStatusCode.OK;
                    fnResult.ReturnData = nonartistResult.Entity;
                }

                return fnResult;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(AddNonArtistToDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<CustomerType>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<AccountImage>> GetAccountImageFromDbAsync(Guid id, AccountImageTypeEnum imageType)
        {
            try
            {
                var accountImage = await _dbContext.AccountImage.FirstOrDefaultAsync(x => x.AppUserId == id && x.AccountImageTypeId == imageType);

                if (accountImage == null)
                {
                    return new ServiceResponse<AccountImage>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<AccountImage>(HttpStatusCode.OK)
                {
                    ReturnData = accountImage
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetAccountImageFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<AccountImage>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<AppUser>> GetAppUserFromDbAsync(Guid id, string memberId)
        {
            try
            {
                var user = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.Id == id && x.MemberId == memberId);

                if (user == null)
                {
                    return new ServiceResponse<AppUser>(HttpStatusCode.Unauthorized);
                }

                return new ServiceResponse<AppUser>(HttpStatusCode.OK)
                {
                    ReturnData = user
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetAppUserFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<AppUser>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Artist>> GetArtistFromDbAsync(Guid id)
        {
            try
            {
                var artist = await _dbContext.Artist
                    .Include(x => x.User)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.EventTypes)
                    .ThenInclude(x => x.EventType)
                    .Include(x => x.Equipment)
                    .Include(x => x.Followers)
                    .ThenInclude(x => x.Follower)
                    .ThenInclude(x => x.NonArtist)
                    .Include(x => x.Followers)
                    .ThenInclude(x => x.Follower)
                    .ThenInclude(x => x.Artist)
                    .Include(x => x.User)
                    .ThenInclude(x => x.Following)
                    .FirstOrDefaultAsync(x => x.AppUserId == id);

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
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Artist>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Artist>> GetArtistFromDbAsync(string profileUrl)
        {
            try
            {
                var artist = await _dbContext.Artist
                    .Include(x => x.User)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.User)
                    .ThenInclude(x => x.Following)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.EventTypes)
                    .ThenInclude(x => x.EventType)
                    .Include(x => x.Equipment)
                    .Include(x => x.Followers)
                    .ThenInclude(x => x.Follower)
                    .ThenInclude(x => x.NonArtist)
                    .Include(x => x.Followers)
                    .ThenInclude(x => x.Follower)
                    .ThenInclude(x => x.Artist)
                    .FirstOrDefaultAsync(x => x.ProfileUrl == profileUrl);

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
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Artist>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<NonArtist>> GetNonArtistFromDbAsync(Guid id)
        {
            try
            {
                var nonArtist = await _dbContext.NonArtist
                    .Include(x => x.User)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.User)
                    .ThenInclude(x => x.Following)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.Images)
                    .FirstOrDefaultAsync(x => x.AppUserId == id);

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
                var message = $"{nameof(AccountRepository)} - {nameof(GetNonArtistFromDbAsync)} - {e.Message}";
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
                var message = $"{nameof(AccountRepository)} - {nameof(GetNonArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<NonArtist>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
