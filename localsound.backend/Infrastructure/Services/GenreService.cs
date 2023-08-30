using AutoMapper;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IGenreRepository genreRepository, IMapper mapper, ILogger<GenreService> logger)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<GenreDto>>> SearchGenreType(string name, CancellationToken cancellationToken)
        {
            try
            {
                var genreResult = await _genreRepository.SearchGenreTypeAsync(name, cancellationToken);

                if (!genreResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse<List<GenreDto>>(genreResult.StatusCode);
                }

                var returnList = _mapper.Map<List<GenreDto>>(genreResult.ReturnData);

                return new ServiceResponse<List<GenreDto>>(HttpStatusCode.OK)
                {
                    ReturnData = returnList
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(GenreService)} - {nameof(SearchGenreType)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<GenreDto>>(HttpStatusCode.InternalServerError, "There was an error while searching for the genre.");
            }
        }
    }
}
