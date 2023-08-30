using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<GenreRepository> _logger;

        public GenreRepository(LocalSoundDbContext dbContext, ILogger<GenreRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<Genre>>> SearchGenreTypeAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                var genre = await _dbContext.Genres.Where(x => x.GenreName.Contains(name)).ToListAsync(cancellationToken);

                return new ServiceResponse<List<Genre>>(HttpStatusCode.OK)
                {
                    ReturnData = genre
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(GenreRepository)} - {nameof(SearchGenreTypeAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<Genre>>(HttpStatusCode.InternalServerError, "There was an error while searching for the genre.");
            }
        }
    }
}
