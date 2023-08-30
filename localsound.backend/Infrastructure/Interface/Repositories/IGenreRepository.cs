using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IGenreRepository
    {
        Task<ServiceResponse<List<Genre>>> SearchGenreTypeAsync(string name, CancellationToken cancellationToken);
    }
}
