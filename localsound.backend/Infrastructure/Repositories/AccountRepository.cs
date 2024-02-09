using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

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

        public async Task<ServiceResponse<CustomerType>> AddArtistToDbAsync(Account artist)
        {
            try
            {
                var fnResult = new ServiceResponse<CustomerType>(HttpStatusCode.BadRequest);

                if (artist is null)
                {
                    return fnResult;
                }

                var result = await _dbContext.Account.FirstOrDefaultAsync(o => o.AppUserId == artist.User.Id);

                if (result != null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(AddArtistToDbAsync)} - Server error adding artist into Artist database - " +
                        $"Id:{artist.User.Id} already exists in customer database";
                    _logger.LogError(message);
                    return fnResult;
                }

                var existingProfileWithUrl = await _dbContext.Account.FirstOrDefaultAsync(o => o.ProfileUrl == artist.ProfileUrl);

                if (existingProfileWithUrl != null)
                {
                    fnResult.ServiceResponseMessage = "That profile URL is already in use, please try another one.";
                }
                else
                {
                    var artistResult = await _dbContext.Account.AddAsync(artist);

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

        public async Task<ServiceResponse<CustomerType>> AddNonArtistToDbAsync(Account nonArtist)
        {
            try
            {
                var fnResult = new ServiceResponse<CustomerType>(HttpStatusCode.BadRequest);

                if (nonArtist is null)
                {
                    return fnResult;
                }

                var result = await _dbContext.Account.FirstOrDefaultAsync(o => o.AppUserId == nonArtist.User.Id);

                if (result != null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(AddNonArtistToDbAsync)} - Server error adding NonArtist into NonArtist database - " +
                        $"Id:{nonArtist.User.Id} already exists in customer database";
                    _logger.LogError(message);
                    return fnResult;
                }

                var existingProfileWithUrl = await _dbContext.Account.FirstOrDefaultAsync(o => o.ProfileUrl == nonArtist.ProfileUrl);

                if (existingProfileWithUrl != null)
                {
                    fnResult.ServiceResponseMessage = "That profile URL is already in use, please try another one.";
                }
                else
                {
                    var nonartistResult = await _dbContext.Account.AddAsync(nonArtist);

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
                var accountImage = await _dbContext.AccountImage.FirstOrDefaultAsync(x => x.AppUserId == id && x.AccountImageTypeId == imageType && !x.ToBeDeleted);

                if (accountImage is null)
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

        public async Task<ServiceResponse<Account>> GetAccountFromDbAsync(string memberId)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.MemberId == memberId);

                if (user is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.Unauthorized);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = user
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(Account)} - {nameof(GetAccountFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Account>> GetAccountFromDbAsync(Guid id)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.AppUserId == id);

                if (user is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.Unauthorized);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = user
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(Account)} - {nameof(GetAccountFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Account>> GetAccountFromDbAsync(Guid id, string memberId)
        {
            try
            {
                var user = await _dbContext.AppUser.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id && x.Account.MemberId == memberId);

                if (user is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.Unauthorized);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = user.Account
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(Account)} - {nameof(GetAccountFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistFollower>>> GetArtistFollowersFromDbAsync(string memberId, int page, CancellationToken cancellationToken)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Followers)
                    .ThenInclude(x => x.Follower)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.Followers)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Skip(page*30)
                    .Take(30)
                    .FirstOrDefaultAsync(x => x.MemberId == memberId);

                if (artist is null)
                {
                    return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.OK)
                {
                    ReturnData = artist.Followers?.Any() == true ? artist.Followers.Select(x => x).ToList() : new List<ArtistFollower>()
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFollowersFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistFollower>>> GetArtistFollowingFromDbAsync(string memberId, int page, CancellationToken cancellationToken)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Following)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Skip(page * 30)
                    .Take(30)
                    .FirstOrDefaultAsync(x => x.MemberId == memberId);

                if (artist is null)
                {
                    return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.OK)
                {
                    ReturnData = artist.Following?.Any() == true ? artist.Following.Select(x => x).ToList() : new List<ArtistFollower>()
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFollowingFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.InternalServerError);
            }
        }

        // Used during login to get artist data from table
        public async Task<ServiceResponse<Account>> GetArtistFromDbAsync(Guid id)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Images)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.EventTypes)
                    .ThenInclude(x => x.EventType)
                    .Include(x => x.Equipment)
                    .Include(x => x.Followers)
                    .Include(x => x.Following)
                    .Include(x => x.Packages)
                    .FirstOrDefaultAsync(x => x.AppUserId == id && x.CustomerType == CustomerTypeEnum.Artist);

                artist.Images = artist.Images.Where(x => !x.ToBeDeleted).ToList();

                if (artist is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = artist
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Account>> GetArtistFromDbAsync(string profileUrl)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Images)
                    .Include(x => x.Following)
                    .Include(x => x.Followers)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.EventTypes)
                    .ThenInclude(x => x.EventType)
                    .Include(x => x.Equipment)
                    .Include(x => x.Packages)
                    .Select(x => new Account
                    {
                        AppUserId = x.AppUserId,
                        User = x.User,
                        Images = x.Images.Where(x => !x.ToBeDeleted).ToList(),
                        Name = x.Name,
                        ProfileUrl = x.ProfileUrl,
                        Address = x.Address,
                        PhoneNumber = x.PhoneNumber,
                        SoundcloudUrl = x.SoundcloudUrl,
                        SpotifyUrl = x.SpotifyUrl,
                        YoutubeUrl = x.YoutubeUrl,
                        AboutSection = x.AboutSection,
                        Genres = x.Genres,
                        EventTypes = x.EventTypes,
                        Equipment = x.Equipment,
                        Followers = x.Followers,
                        Packages = x.Packages.Where(x => x.IsAvailable).ToList(),

                    })
                    .FirstOrDefaultAsync(x => x.ProfileUrl == profileUrl);

                if (artist is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = artist
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Account>> GetNonArtistFromDbAsync(Guid id)
        {
            try
            {
                var nonArtist = await _dbContext.Account
                    .Include(x => x.Images)
                    .Include(x => x.Following)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .FirstOrDefaultAsync(x => x.AppUserId == id);

                if (nonArtist is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = nonArtist
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetNonArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Account>> GetNonArtistFromDbAsync(string profileUrl)
        {
            try
            {
                var nonArtist = await _dbContext.Account.Include(x => x.User).FirstOrDefaultAsync(x => x.ProfileUrl == profileUrl);

                if (nonArtist is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<Account>(HttpStatusCode.OK)
                {
                    ReturnData = nonArtist
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetNonArtistFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
