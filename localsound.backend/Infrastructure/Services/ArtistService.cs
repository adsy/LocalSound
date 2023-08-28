using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(IArtistRepository artistRepository, LocalSoundDbContext dbContext, ILogger<ArtistService> logger)
        {
            _artistRepository = artistRepository;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> UpdateArtistDetails(Guid userId, string memberId, UpdateArtistDto updateArtistDto)
        {
            try
            {
                var appUser = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.MemberId == memberId);

                if (appUser == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "There was an error while updating your details, please try again.");
                }

                if (appUser.Id != userId)
                {
                    return new ServiceResponse(HttpStatusCode.Unauthorized, "There was an error while updating your details, please try again.");
                }

                return await _artistRepository.UpdateArtistDetailsAsync(userId, updateArtistDto);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(UpdateArtistDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }
    }
}
