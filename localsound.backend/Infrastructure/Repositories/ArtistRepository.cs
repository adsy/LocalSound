using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<ArtistRepository> _logger;

        public ArtistRepository(LocalSoundDbContext dbContext, ILogger<ArtistRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> UpdateArtistDetailsAsync(Guid userId, UpdateArtistDto updateArtistDto)
        {
            try
            {
                var artist = await _dbContext.Artist.Include(x => x.Genres).FirstOrDefaultAsync(x => x.AppUserId == userId);

                // Artist should not be null here, otherwise its an issue with the artist creation/DB
                if (artist == null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistDetailsAsync)} - Could not find matching artist with userId: {userId}";
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
                }

                var artistGenres = updateArtistDto.Genres.Select(x => new ArtistGenre
                {
                    AppUserId = artist.AppUserId,
                    GenreId = x.GenreId
                }).ToList();

                artist.UpdateName(updateArtistDto.Name)
                    .UpdateAddress(updateArtistDto.Address)
                    .UpdatePhoneNumber(updateArtistDto.PhoneNumber)
                    .UpdateProfileUrl(updateArtistDto.ProfileUrl)
                    .UpdateSocialLinks(updateArtistDto.SoundcloudUrl, updateArtistDto.SpotifyUrl, updateArtistDto.YoutubeUrl)
                    .UpdateAboutSection(updateArtistDto.AboutSection)
                    .UpdateGenres(artistGenres);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistDetailsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }
    }
}
