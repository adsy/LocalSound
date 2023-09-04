using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.Genre
{
    public class SearchGenreQuery : IRequest<ServiceResponse<List<GenreDto>>>
    {
        public string Name { get; set; }
    }
}
