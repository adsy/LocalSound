using localsound.backend.Domain.Model;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IBlobRepository
    {
        Task<ServiceResponse> DeleteBlobAsync(string fileLocation);
        Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation, IFormFile file);
    }
}
