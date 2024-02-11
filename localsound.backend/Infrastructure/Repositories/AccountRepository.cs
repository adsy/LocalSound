using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
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
                var artistFollowers = await _dbContext.ArtistFollower
                    .Include(x => x.Follower)
                    .ThenInclude(x => x.Images)
                    .Where(x => x.Artist.MemberId == memberId)
                    .ToListAsync(cancellationToken);

                if (artistFollowers is null)
                {
                    return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.OK)
                {
                    ReturnData = artistFollowers
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetArtistFollowersFromDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistFollower>>> GetProfileFollowingFromDbAsync(string memberId, int page, CancellationToken cancellationToken)
        {
            try
            {
                var followingList = await _dbContext.ArtistFollower
                    .Include(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Where(x => x.Follower.MemberId == memberId)
                    .ToListAsync();

                if (followingList is null)
                {
                    return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<List<ArtistFollower>>(HttpStatusCode.OK)
                {
                    ReturnData = followingList
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(GetProfileFollowingFromDbAsync)} - {e.Message}";
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
                    .Include(x => x.AccountOnboarding)
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


                if (artist is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound);
                }

                artist.Images = artist.Images.Where(x => !x.ToBeDeleted).ToList();

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
                    .FirstOrDefaultAsync(x => x.ProfileUrl == profileUrl);

                if (artist is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound);
                }

                artist.Images = artist.Images.Where(x => !x.ToBeDeleted).ToList();

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
                    .Include(x => x.AccountOnboarding)
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

        public async Task<ServiceResponse> SaveOnboardingData(Guid userId, SaveOnboardingDataDto onboardingData)
        {
            try
            {
                var account = await _dbContext.Account.Include(x => x.AccountOnboarding).Include(x => x.Genres).Include(x => x.EventTypes).Include(x => x.Equipment).FirstOrDefaultAsync(x => x.AppUserId == userId);

                if (account is null)
                {
                    return new ServiceResponse<Account>(HttpStatusCode.NotFound)
                    {
                        ServiceResponseMessage = "There was an error saving your data, please try again..."
                    };
                }

                account.UpdateAboutSection(onboardingData.AboutSection).UpdateAccountSetupCompleted();

                if (onboardingData.Genres.Any())
                {
                    account.UpdateGenres(onboardingData.Genres.Select(x => new AccountGenre
                    {
                        AppUserId = userId,
                        GenreId = x.GenreId
                    }).ToList());
                }

                if (onboardingData.Equipment is not null && onboardingData.Equipment.Any())
                {
                    account.UpdateEquipment(onboardingData.Equipment.Select(x => new ArtistEquipment
                    {
                        AppUserId = userId,
                        EquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName
                    }).ToList());
                }

                if (onboardingData.EventTypes is not null && onboardingData.EventTypes.Any())
                {
                    account.UpdateEventTypes(onboardingData.EventTypes.Select(x => new ArtistEventType
                    {
                        AppUserId = userId,
                        EventTypeId = x.EventTypeId
                    }).ToList());
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(SaveOnboardingData)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<Account>(HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResponse> UpdateArtistFollowerAsync(Account follower, string artistId, bool startFollowing)
        {
            var followString = startFollowing ? "following" : "unfollowing";
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.MemberId == artistId);

                if (artist is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, $"There was an error while {followString} the artist, please try again.");
                }

                if (startFollowing)
                {
                    var artistFollower = await _dbContext.ArtistFollower.FirstOrDefaultAsync(x => x.ArtistId == artist.AppUserId && x.FollowerId == follower.AppUserId);

                    if (artistFollower != null)
                    {
                        return new ServiceResponse(HttpStatusCode.InternalServerError, $"There was an error while {followString} the artist, please try again.");
                    }

                    await _dbContext.ArtistFollower.AddAsync(new ArtistFollower
                    {
                        ArtistId = artist.AppUserId,
                        FollowerId = follower.AppUserId
                    });
                }
                else
                {
                    var artistFollower = await _dbContext.ArtistFollower.FirstOrDefaultAsync(x => x.ArtistId == artist.AppUserId && x.FollowerId == follower.AppUserId);

                    if (artistFollower is null)
                    {
                        return new ServiceResponse(HttpStatusCode.NotFound, $"There was an error while {followString} the artist, please try again.");
                    }

                    _dbContext.ArtistFollower.Remove(artistFollower);
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistFollowerAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, $"There was an error while {followString} the artist, please try again.");
            }
        }

        public async Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, UpdateArtistPersonalDetailsDto updateArtistDto)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Genres)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId);

                // Artist should not be null here, otherwise its an issue with the artist creation/DB
                if (artist is null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistPersonalDetails)} - Could not find matching artist with userId: {userId}";
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
                }

                artist.UpdateName(updateArtistDto.Name)
                    .UpdateAddress(updateArtistDto.Address)
                    .UpdatePhoneNumber(updateArtistDto.PhoneNumber)
                    .UpdateProfileUrl(updateArtistDto.ProfileUrl)
                    .UpdateSocialLinks(updateArtistDto.SoundcloudUrl, updateArtistDto.SpotifyUrl, updateArtistDto.YoutubeUrl)
                    .UpdateAboutSection(updateArtistDto.AboutSection);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistPersonalDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }

        public async Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, UpdateArtistProfileDetailsDto updateArtistDto)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Genres)
                    .Include(x => x.EventTypes)
                    .Include(x => x.Equipment)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId);

                // Artist should not be null here, otherwise its an issue with the artist creation/DB
                if (artist is null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistProfileDetails)} - Could not find matching artist with userId: {userId}";
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
                }

                var artistGenres = updateArtistDto.Genres.Select(x => new AccountGenre
                {
                    AppUserId = artist.AppUserId,
                    GenreId = x.GenreId
                }).ToList();

                var eventTypes = updateArtistDto.EventTypes.Select(x => new ArtistEventType
                {
                    AppUserId = artist.AppUserId,
                    EventTypeId = x.EventTypeId
                }).ToList();

                var equipment = updateArtistDto.Equipment.Select(x => new ArtistEquipment
                {
                    AppUserId = artist.AppUserId,
                    EquipmentId = x.EquipmentId,
                    EquipmentName = x.EquipmentName
                }).ToList();

                artist.UpdateGenres(artistGenres)
                    .UpdateEventTypes(eventTypes)
                    .UpdateEquipment(equipment);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistProfileDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }
    }
}
