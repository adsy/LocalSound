using localsound.backend.api.Queries.Genre;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Genre
{
    public class GenreHandler : IRequestHandler<SearchGenreQuery, ServiceResponse<List<GenreDto>>>
    {
        private readonly IGenreService _genreService;

        public GenreHandler(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<ServiceResponse<List<GenreDto>>> Handle(SearchGenreQuery request, CancellationToken cancellationToken)
        {
            return await _genreService.SearchGenreType(request.Name, cancellationToken);
        }
    }
}
