using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(IArtistRepository artistRepository, ILogger<ArtistService> logger, IAccountRepository accountRepository)
        {
            _artistRepository = artistRepository;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, string memberId, UpdateArtistPersonalDetailsDto updateArtistDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "There was an error while updating your details, please try again.");
                }

                return await _artistRepository.UpdateArtistPersonalDetails(userId, updateArtistDto);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(UpdateArtistPersonalDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }

        public async Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, string memberId, UpdateArtistProfileDetailsDto updateArtistDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "There was an error while updating your details, please try again.");
                }

                return await _artistRepository.UpdateArtistProfileDetails(userId, updateArtistDto);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(UpdateArtistProfileDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }

        public async Task<ServiceResponse> FollowArtist(Guid userId, string followerId, string artistId)
        {
            try
            {
                var accountResult = await _accountRepository.GetAppUserFromDbAsync(userId, followerId);

                if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData == null)
                {
                    return new ServiceResponse(accountResult.StatusCode);
                }

                var followResult = await _artistRepository.FollowArtistAsync(accountResult.ReturnData, artistId);

                if (!followResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(accountResult.StatusCode);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(ArtistService)} - {nameof(FollowArtist)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
