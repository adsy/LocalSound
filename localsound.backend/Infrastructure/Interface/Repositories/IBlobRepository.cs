using localsound.backend.Domain.Model;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IBlobRepository
    {
        Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation);
    }
}
